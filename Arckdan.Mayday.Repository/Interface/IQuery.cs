using Arckdan.Mayday.Services.Mensagem.Models.Sistema;

namespace Arckdan.Mayday.Repository.Interface
{
    public interface IQuery
    {
        #region métodos

        Retorno Listar();
        Retorno Listar(string? where);
        Retorno Obter(Guid id);

        #endregion
    }
}
