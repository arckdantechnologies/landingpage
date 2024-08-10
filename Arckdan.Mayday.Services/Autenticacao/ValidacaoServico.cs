using Arckdan.Mayday.Domain.Seguranca;
using Arckdan.Mayday.Services.Autenticacao.Interface;
using Arckdan.Mayday.Services.Mensagem.Enums;
using Arckdan.Mayday.Services.Mensagem.Models.Sistema;
using Arckdan.Mayday.Services.Token.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Arckdan.Mayday.Services.Autenticacao
{
    public class ValidacaoServico : IValidacaoServico
    {
        #region atributos

        private readonly TokenSettingsModel _tokenSettings;
        private readonly IList<TokenClientModel> _listaTokenClient;

        #endregion

        #region construtores

        /// <summary>
        /// construtor da classe ValidacaoServico
        /// </summary>
        /// <param name="tokenSettings"></param>
        public ValidacaoServico(TokenSettingsModel tokenSettings)
        {
            _tokenSettings = tokenSettings;
            _listaTokenClient = new List<TokenClientModel>()
            {
                TokenClientModel.TokenClientFactory.ObterModel(_tokenSettings.Client_Id, _tokenSettings.Client_Secret, _tokenSettings.Client_Role)
            };
        }

        #endregion

        /// <summary>
        /// método utilizado para validar o usuário
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public TokenClientModel ValidarUsuario(string clientId, string clientSecret)
            => _listaTokenClient.Where(x => x.ClientId == clientId && x.ClientSecret == clientSecret).FirstOrDefault();


        public Retorno ValidarEmail(string email)
        {
            // bloco de construçao de objetos
            var emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$";

            // condição para validar o endereço de e-mail
            if (!Regex.IsMatch(email, emailRegex))
                return new Validacao(ERetorno.Erro, EValidacao.Email);

            return new Validacao(ERetorno.Sucesso, EValidacao.Email);
        }
    }
}
