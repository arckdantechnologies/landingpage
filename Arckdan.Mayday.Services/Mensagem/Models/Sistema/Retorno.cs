using Arckdan.Mayday.Services.Mensagem.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Resources;
using System.Text.Json.Serialization;

namespace Arckdan.Mayday.Services.Mensagem.Models.Sistema
{
    public abstract class Retorno
    {
        #region constantes

        protected const string Separador = "_";
        private ResourceManager _resourceManager =>
            new ResourceManager("Arckdan.Mayday.Services.Mensagem.Resource.pt-BR", Assembly.GetExecutingAssembly());

        #endregion

        #region métodos

        public Retorno() { }

        public Retorno(ERetorno codigo, EValidacao validacao, string mensagemErro) 
        {
            this.Codigo = codigo;
            this.Mensagem = mensagemErro;
            this.Validacao = validacao;
        }

        /// <summary>
        /// método utilizado para carregar o conjunto de chave e valor para a mensagem de retorno do processamnto
        /// </summary>
        /// <param name="codigo">código de retorno</param>
        /// <param name="chave">chave de pesquisa para a mensagem</param>
        /// <param name="mensagemErro">mensagem complemento ao tratar o erro de processamento</param>
        public Retorno(ERetorno codigo, string chave, string? mensagemErro = "")
        {
            // carrega o conjunto de chave e valor para os propriedades da mensagem de retorno
            Codigo = codigo;
            chave = string.Join(Separador, chave, codigo.ToString().ToUpper());

            // condição para complementar a mensagem de erro
            if (codigo == ERetorno.Erro)
                if (mensagemErro == "")
                    Mensagem = ObterMensagem(chave);
                else
                    Mensagem = string.Join(Separador, ObterMensagem(chave), mensagemErro);
            else
                Mensagem = ObterMensagem(chave);
        }

        /// <summary>
        /// método utilizado para obter a mensagem de retorno do sistema
        /// </summary>
        /// <param name="chave">chave de acesso à mensagem de retorno</param>
        /// <returns>retorna a mensagem do sistema</returns>
        private string? ObterMensagem(string chave)
            => _resourceManager.GetString(chave) is null ? chave : _resourceManager.GetString(chave);

        #endregion

        #region propriedades

        [JsonPropertyName("validacao")]
        [Column(Order = 0)]
        public EValidacao Validacao { get; private set; }

        [JsonPropertyName("codigo")]
        [Column(Order = 1)]
        public ERetorno Codigo { get; private set; }

        [JsonPropertyName("mensagem")]
        [Column(Order = 2)]
        public string? Mensagem { get; private set; }

        #endregion
    }
}
