using Arckdan.Mayday.Services.Mensagem.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Arckdan.Mayday.Services.Mensagem.Models.Sistema
{
    public class Registro<T> : Retorno where T : class
    {
        #region atributos

        public const string LISTAGEM = "LISTA";

        #endregion

        #region construtores

        public Registro(ERetorno codigo, string? mensagemErro = "", IEnumerable<T>? lista = null)
            : base(codigo, LISTAGEM, mensagemErro)
        {
            Lista = lista;
        }

        #endregion

        #region propriedades

        [JsonPropertyName("lista")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [Column(Order = 3)]
        public IEnumerable<T>? Lista { get; private set; }

        #endregion
    }
}
