using Arckdan.Mayday.Services.Mensagem.Models.Sistema;

namespace Arckdan.Mayday.Services.Mensagem.Interface
{
    public interface IEmailServico
    {
        #region métodos

        Retorno Enviar(string email, string nomeDestino, string token, bool html = true);

        #endregion
    }
}
