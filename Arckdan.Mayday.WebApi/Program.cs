using Arckdan.Mayday.Domain.Comunidade;
using Arckdan.Mayday.Repository.Command.Comunidade;
using Arckdan.Mayday.Repository.Context;
using Arckdan.Mayday.Repository.Interface;
using Arckdan.Mayday.Repository.Query;
using Arckdan.Mayday.Repository.Query.Comunidade;
using Arckdan.Mayday.Services.Autenticacao;
using Arckdan.Mayday.Services.Autenticacao.Interface;
using Arckdan.Mayday.Services.Mensagem;
using Arckdan.Mayday.Services.Mensagem.Enums;
using Arckdan.Mayday.Services.Mensagem.Interface;
using Arckdan.Mayday.Services.Mensagem.Models.Sistema;
using Arckdan.Mayday.Services.Token;
using Arckdan.Mayday.Services.Token.Interface;
using Arckdan.Mayday.Services.Token.Models;
using Arckdan.Mayday.UnityOfWork;
using Arckdan.Mayday.WebApi.Models.Comunidade;
using Arckdan.Mayday.WebApi.Models.Seguranca;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var MaydayMySqlDev = builder.Configuration.GetConnectionString("MaydayMySqlDev");
builder.Services.AddDbContext<MaydayMySqlContext>(options => options.UseMySql(MaydayMySqlDev, ServerVersion.Parse("9.0.0")));
builder.Services.AddTransient<MySqlSession>();
builder.Services.AddTransient<TokenSettingsModel>();
builder.Services.AddTransient<ValidacaoLandingPageServico>();
builder.Services.AddTransient<JwtSecurityTokenHandler>();
builder.Services.AddTransient<ITokenServico, TokenServico>();
builder.Services.AddTransient<IValidacaoServico, ValidacaoServico>();
builder.Services.AddTransient<IUoW, UoW>();
builder.Services.AddTransient<IEmailServico, EmailServico>();
builder.Services.AddTransient<ICommand<LandingPageModel>, LandingPageCommand>();
builder.Services.AddTransient<IQuery, LandingPageQuery>();
builder.Services.AddTransient<ITokenServico, TokenServico>();

// automapper
var config = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<LandingPageViewModel, LandingPageModel>().ReverseMap();
});
IMapper mapper = config.CreateMapper();

// configuração da autenticação das operações de apis via JWT
TokenSettingsModel tokenSettings = new TokenSettingsModel(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = tokenSettings.Issuer,
            ValidAudience = tokenSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// apis para gerar o token padrão JWT
app.MapPost("api/v1/token/gerar", (ITokenServico tokenServico, [FromBody] TokenViewModel p) =>
{
    // bloco de tratamento de exceção
    try
    {
        // gera o token padrão JWT bearer
        var jsonData = tokenServico.GerarTokenJWT(p.Client_Id, p.Client_Secret);

        // condição para o retorno do processo de geração do token JWT
        if (jsonData.Codigo == ERetorno.Sucesso)
            return Results.Ok(jsonData);
        else
            return Results.BadRequest(jsonData);
    }
    catch (Exception sysEx)
    {
        return Results.BadRequest(new Validacao(ERetorno.Erro, EValidacao.Token, null, sysEx.Message));
    }
}).WithTags("Token");

// apis para o cadastro dos dados na landing page
app.MapPost("api/v1/landingpage/incluir", [Authorize] async ([FromServices] ITokenServico tokenServico, IValidacaoServico validacaoServico, [FromServices] IUoW uow, [FromBody] LandingPageViewModel p) =>
{
    // bloco de construção de objetos
    EValidacao validacao = EValidacao.Cadastro;

    // bloco de tratamento de exceção
    try
    {
        // bloco de declaração de variáveis
        var entity = mapper.Map<LandingPageModel>(p);

        // validação de campos obrigatórios
        var validarLandingPage = new ValidacaoLandingPageServico();
        var resultadoValidacao = validarLandingPage.Validate(entity);

        if (!resultadoValidacao.IsValid)
        {
            var resultado = new List<Retorno>();
            foreach (var erro in resultadoValidacao.Errors)
                resultado.Add(new Validacao(ERetorno.Erro, EValidacao.Cadastro, erro.ErrorMessage));
            return Results.BadRequest(resultado);
        }

        // condição para retornar a validação do endereço de e-mail
        var validacaoEmail = validacaoServico.ValidarEmail(p.Email);
        if (validacaoEmail.Codigo == ERetorno.Erro)
            return Results.BadRequest(validacaoEmail);

        // inicia a transação com o banco de dados
        uow.BeginTransaction();

        // condição para gravar ou atualizar os dados
        if (p.Alteracao == null)
        {
            // gera o token de usuário
            tokenServico.GerarTokenUsuario(entity);

            // incluir os dados do early adopter
            uow.LandingPageCommand.Incluir(entity);
        }
        else
        {
            // atualizar os dados do early adopter
            validacao = EValidacao.Atualizacao;
            uow.LandingPageCommand.Alterar(entity);
        }
        uow.Commit();

        return Results.Ok(new Cadastro(ERetorno.Sucesso, validacao, string.Empty));
    }
    catch (Exception ex)
    {
        uow.Rollback();
        return Results.BadRequest(new Cadastro(ERetorno.Erro, validacao, ex.Message));
    }
})
.WithTags("Landing Page");

app.MapGet("api/v1/landingpage/listar", [Authorize] ([FromServices] IQuery query) =>
{
    // bloco de construção de objetos
    var resultado = query.Listar();

    // bloco de tratamento do processamento da listagem
    switch (resultado.Codigo)
    {
        case ERetorno.Sucesso:
            return Results.Ok(resultado);
        default:
            return Results.BadRequest(resultado);
    }
})
.WithTags("Landing Page");

app.MapGet("api/v1/landingpage/{id}/obter", [Authorize] ([FromServices] IQuery query, Guid id) =>
{
    // bloco de construção de objetos
    var resultado = query.Obter(id);

    // bloco de tratamento do processamento da listagem
    switch (resultado.Codigo)
    {
        case ERetorno.Sucesso:
            return Results.Ok(resultado);
        default:
            return Results.BadRequest(resultado);
    }
})
.WithTags("Landing Page");

app.MapGet("api/v1/landingpage/{where}/listar", [Authorize] ([FromServices] IQuery query, string where) =>
{
    // bloco de construção de objetos
    var resultado = query.Listar(where);

    // condição para tratar o retorno dos dados
    if (resultado.Codigo == ERetorno.Sucesso)
        return Results.Ok(resultado);
    else
        return Results.BadRequest(resultado);
})
.WithTags("Landing Page");

app.MapPost("api/v1/landingpage/email/{nome}/{email}/{token}", [Authorize] ([FromServices] IEmailServico servico, string nome, string email, string token) =>
{
    // método utilizado para processar o envio de email
    var retorno = servico.Enviar(nome, email, token);

    if (retorno.Codigo == ERetorno.Sucesso)
        return Results.Ok(retorno);
    else
        return Results.BadRequest(retorno);
})
.WithTags("Landing Page");


app.Run();