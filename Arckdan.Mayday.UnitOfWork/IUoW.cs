using Arckdan.Mayday.Domain.Comunidade;
using Arckdan.Mayday.Repository.Interface;

namespace Arckdan.Mayday.UnityOfWork
{
    public interface IUoW : IDisposable
    {
        #region métodos

        void BeginTransaction();
        void Commit();
        void Rollback();

        #endregion

        #region propriedades

        ICommand<LandingPageModel> LandingPageCommand { get; }

        #endregion
    }
}
