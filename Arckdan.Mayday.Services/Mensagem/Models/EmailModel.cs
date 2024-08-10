using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Arckdan.Mayday.Services.Mensagem.Models
{
    public class EmailModel
    {
        #region construtores

        public EmailModel() { }

        #endregion

        #region propriedades

        public string? SMTP { get; private set; }
        public int Porta { get; private set; }
        public string? EmailRemetente { get; private set; }
        public string? Senha { get; private set; }
        public string EmailDestino { get; private set; }
        public string? NomeRemetente { get; private set; }
        public string? NomeDestino { get; private set; }

        [JsonPropertyName("assunto")]
        public string? Assunto { get; set; }

        [JsonPropertyName("mensagem")]
        public string? Mensagem  { get; set; }
        public bool Html { get; private set; }

        #endregion

        public static class EmailServicoFactory
        {
            #region factory

            public static EmailModel ObterModel(string emailDestino, string nomeDestino, bool html, IConfiguration configuration, string? emailMensagem)
                => new EmailModel
                {
                    SMTP = configuration.GetSection("Smtp:Server").Value,
                    Porta = Convert.ToInt32(configuration.GetSection("Smtp:Port").Value),
                    Senha = configuration.GetSection("Smtp:Password").Value,
                    EmailRemetente = configuration.GetSection("Smtp:FromAddress").Value,
                    EmailDestino = emailDestino,
                    NomeDestino = nomeDestino,
                    Assunto = ObterDadosMensagem(emailMensagem).Assunto,
                    Mensagem = ObterDadosMensagem(emailMensagem).Mensagem,
                    Html = html
                };

            private static EmailModel? ObterDadosMensagem(string emailMensagem)
            {
                return JsonSerializer.Deserialize<EmailModel>(emailMensagem);                
            }

            #endregion
        }
    }
}
