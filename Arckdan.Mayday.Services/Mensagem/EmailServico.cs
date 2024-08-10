using Arckdan.Mayday.Services.Autenticacao.Interface;
using Arckdan.Mayday.Services.Mensagem.Interface;
using Arckdan.Mayday.Services.Mensagem.Models;
using Arckdan.Mayday.Services.Mensagem.Models.Sistema;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Resources;

namespace Arckdan.Mayday.Services.Mensagem
{
    public class EmailServico : IEmailServico
    {
        #region atributos

        private readonly IConfiguration _configuration;
        private readonly IValidacaoServico _validacaoServico;

        #endregion

        #region constantes

        private ResourceManager _resourceManager =>
            new ResourceManager("Arckdan.Mayday.Services.Mensagem.Resource.pt-BR", Assembly.GetExecutingAssembly());

        #endregion

        #region construtores

        /// <summary>
        /// construtor da classe EmailServico
        /// </summary>
        /// <param name="configuration"></param>
        public EmailServico(IConfiguration configuration)
        {
            // bloco de injeção de dependência
            _configuration = configuration;
        }

        #endregion

        #region métodos
        public Retorno Enviar(string nomeDestino, string emailDestino, string token, bool html = true)
        {
            // bloco de declaração de variáveis
            string? emailMensagem = _resourceManager.GetString("EMAIL_LANDINGPAGE");
            var p = EmailModel.EmailServicoFactory.ObterModel(emailDestino, nomeDestino, html, _configuration, emailMensagem);

            // bloco de tratamento de exceção
            try
            {
                // configuração de acesso ao serviço de envio de e-mail
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    // configurações para acesso à conta SMTP
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(p.EmailRemetente);
                        mailMessage.To.Add(emailDestino);
                        mailMessage.Subject = p.Assunto;
                        mailMessage.Body = p.Mensagem.Replace("$nome$", nomeDestino).Replace("$token$", token);
                        mailMessage.IsBodyHtml = html;
                        mailMessage.Priority = MailPriority.Normal;

                        // configurações do servidor SMTP
                        NetworkCredential networkCredential = new NetworkCredential(p.EmailRemetente, p.Senha);
                        smtpClient.Host = p.SMTP;
                        smtpClient.Port = p.Porta;
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = networkCredential;

                        // enviar a mensagem de email
                        smtpClient.Send(mailMessage);

                        return new Email(Enums.ERetorno.Sucesso);
                    }
                }
            }
            catch
            {
                return new Email(Enums.ERetorno.Erro);
            }
        }

        #endregion
    }
}
