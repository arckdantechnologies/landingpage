using Arckdan.Mayday.Domain.Comunidade;
using Arckdan.Mayday.Domain.Seguranca;
using Arckdan.Mayday.Services.Mensagem.Models.Sistema;

namespace Arckdan.Mayday.Services.Token.Interface
{
    public interface ITokenServico
    {
        #region métodos

        void GerarTokenUsuario(LandingPageModel p);
        Retorno GerarTokenJWT(string clientId, string clientRole);
        string RegerarToken();

        #endregion
    }
}
