using Arckdan.Mayday.Domain.Seguranca;
using Arckdan.Mayday.Services.Mensagem.Models.Sistema;

namespace Arckdan.Mayday.Services.Autenticacao.Interface
{
    public interface IValidacaoServico
    {
        #region métodos

        TokenClientModel ValidarUsuario(string clientId, string clientSecret);

        Retorno ValidarEmail(string email);

        #endregion
    }
}
