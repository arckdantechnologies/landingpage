using Arckdan.Mayday.Services.Mensagem.Enums;

namespace Arckdan.Mayday.Services.Mensagem.Models.Sistema
{
    public class Cadastro : Retorno
    {
        #region construtores

        public Cadastro(ERetorno codigo, EValidacao validacao, string? mensagemErro = "")
            : base(codigo, validacao.ToString().ToUpper(), mensagemErro)
        {
        }

        #endregion
    }
}
