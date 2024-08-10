namespace Arckdan.Mayday.Repository.Interface
{
    public interface ICommand<T> where T : class
    {
        #region métodos

        void Incluir(T p);
        void Alterar(T p);
        void Excluir(Guid id);

        #endregion
    }
}
