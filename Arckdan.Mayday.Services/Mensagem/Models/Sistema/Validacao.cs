using Arckdan.Mayday.Domain.Seguranca;
using Arckdan.Mayday.Services.Mensagem.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Arckdan.Mayday.Services.Mensagem.Models.Sistema
{
    public class Validacao : Retorno
    {
        #region atributos

        private const string VALIDACAO = "VALIDACAO";

        #endregion

        #region construtores

        /// <summary>
        /// construtor da classe Validacao
        /// </summary>
        public Validacao(ERetorno codigo, EValidacao validacao, TokenBearerModel? p = null, string? mensagemErro = "") : base(codigo, string.Join(Separador, VALIDACAO, validacao.ToString().ToUpper()), mensagemErro)
        {
            // condição para carregar os dados do token JWT
            if (p != null)
                TokenJWT = TokenBearerModel.TokenModelFactory.ObterModel(p.AccessToken, p.TokenType, p.ExpiresIn, p.RefreshToken);
        }

        public Validacao(ERetorno codigo, EValidacao validacao, string mensagemErro) : base(codigo, validacao, mensagemErro)
        {

        }

        #endregion

        #region propriedades

        [JsonPropertyName("tokenJWT")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Column(Order = 3)]
        public TokenBearerModel TokenJWT { get; private set; }

        #endregion
    }
}
