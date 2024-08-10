using Arckdan.Mayday.Services.Mensagem.Enums;

namespace Arckdan.Mayday.Services.Mensagem.Models.Sistema
{
    public class Email : Retorno
    {
        #region atributos

        public const string EMAIL = "EMAIL";

        #endregion

        #region construtores

        public Email(ERetorno codigo, string? mensagemErro = "")
            : base(codigo, EMAIL, mensagemErro)
        {
        }

        #endregion
    }
}
