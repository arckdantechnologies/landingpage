using Arckdan.Mayday.Domain.Comunidade;
using Arckdan.Mayday.Domain.Seguranca;
using Arckdan.Mayday.Services.Autenticacao.Interface;
using Arckdan.Mayday.Services.Mensagem.Enums;
using Arckdan.Mayday.Services.Mensagem.Models.Sistema;
using Arckdan.Mayday.Services.Token.Interface;
using Arckdan.Mayday.Services.Token.Models;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Arckdan.Mayday.Services.Token
{
    public class TokenServico : ITokenServico
    {
        #region atributos

        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenSettingsModel _tokenSettings;
        private readonly IValidacaoServico _validacaoServico;

        #endregion

        #region construtores

        /// <summary>
        /// construtor da classe TokenServico
        /// </summary>
        /// <param name="validacaoServico">validação do usuário para a geração do token JWT</param>
        /// <param name="tokenHandler">criação do token JWT</param>
        /// <param name="tokenSettings">configurações dos dados para gerar o token JWT</param>
        public TokenServico(IValidacaoServico validacaoServico, JwtSecurityTokenHandler tokenHandler, TokenSettingsModel tokenSettings) 
        { 
            // bloco de injeção de dependência
            _validacaoServico = validacaoServico;
            _tokenHandler = tokenHandler;
            _tokenSettings = tokenSettings;
        }

        #endregion

        #region métodos

        /// <summary>
        /// método utilizado para gerar o token padrão JWT
        /// </summary>
        /// <param name="clientId">id do usuário para gerar o token</param>
        /// <param name="clientRole">papel do usuário gerador do token</param>
        /// <returns>retorna a mensagem do token conforme usuário validado</returns>
        public Retorno GerarTokenJWT(string clientId, string clientSecret)
        {
            // bloco de construção de objetos
            var credenciaisToken = _validacaoServico.ValidarUsuario(clientId, clientSecret);

            // condição para validar as credenciais para validar o usuário de geração do token JWT
            if (credenciaisToken == null)
                return new Validacao(ERetorno.Erro, EValidacao.Usuario);

            var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_tokenSettings.Key));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, credenciaisToken.ClientId),
                    new Claim(ClaimTypes.Role, credenciaisToken.ClientRole),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = _tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var randomNumber = new byte[32];
            var rng = RandomNumberGenerator.Create();
            var refreshToken = RegerarToken();

            // obtém o código randômico para regerar o token
            rng.GetBytes(randomNumber);

            var bearerToken = TokenBearerModel.TokenModelFactory.ObterModel(accessToken, "bearer", tokenDescriptor.Expires.Value.Second, refreshToken);

            return new Validacao(ERetorno.Sucesso, EValidacao.Usuario, bearerToken);
        }

        /// <summary>
        /// atualiza o token
        /// </summary>
        /// <returns>retorna um novo token gerado</returns>
        public string RegerarToken()
        {
            // bloco de construção de objetos
            var randomNumber = new byte[32];
            var rng = RandomNumberGenerator.Create();

            // obtém o código randômico para regerar o token
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// método utilizado para gerar o token de segurança do usuário
        /// </summary>
        /// <param name="id">id do early adopter</param>
        /// <param name="nome">nome do early adopter</param>
        /// <param name="inclusao">data de inclusão do registro</param>
        public void GerarTokenUsuario(LandingPageModel p)
        {
            // bloco de construção de objetos
            var dateConvert = Convert.ToDateTime(p.Inclusao);
            string day = Convert.ToString(dateConvert.Day),
                   month = Convert.ToString(dateConvert.Month),
                   year = Convert.ToString(dateConvert.Year),
                   hour = Convert.ToString(dateConvert.Hour),
                   minute = Convert.ToString(dateConvert.Minute),
                   second = Convert.ToString(dateConvert.Second);

            var stringBuilder = new StringBuilder()
                .Append(p.Id)
                .Append(RemoverCaracteresEspeciais(p.Nome))
                .Append(day)
                .Append(month)
                .Append(year)
                .Append(hour)
                .Append(minute)
                .Append(second);

            // gera o token de acesso do usuário
            byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
            LandingPageModel.LandingPageFactory.ObterModel(Convert.ToBase64String(bytes), p);
        }

        /// <summary>
        /// método utilizado para remover os caracteres especiais e acentos
        /// </summary>
        /// <param name="texto">texto que será tratado para a remoção dos caracteres e acentos</param>
        /// <returns>retorna o texto tratado</returns>
        /// <exception cref="ArgumentNullException">exceção gerada quando não houver o texto informado</exception>
        private string RemoverCaracteresEspeciais(string texto)
        {
            // bloco de construção de objetos
            var sb = new StringBuilder();

            // condição para verificar se o texto é nulo
            if (texto == null)
                throw new ArgumentNullException(nameof(texto));
        
            // normaliza a string para decomposição
            string normalizedString = texto.Normalize(NormalizationForm.FormD);            

            // implementa um laço para remover os caracteres diacríticos (acentos) e outros caracteres especiais
            foreach (char c in normalizedString)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            // normaliza para FormC e retorna a string resultante
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        #endregion
    }
}
