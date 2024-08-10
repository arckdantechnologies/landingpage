using Arckdan.Mayday.Domain.Comunidade;
using Arckdan.Mayday.Repository.Interface;
using Arckdan.Mayday.Repository.Query;
namespace Arckdan.Mayday.UnityOfWork
{
    public sealed class UoW : IUoW
    {
        #region atributos

        private readonly MySqlSession _mySqlSession;
        private readonly ICommand<LandingPageModel> _landingPageCommand;
        
        #endregion

        #region construtor

        /// <summary>
        /// construtor da classe UoW
        /// </summary>
        /// <param name="mySqlSession">busca as configurações para o acesso ao banco de dados mysql</param>
        public UoW(MySqlSession mySqlSession, ICommand<LandingPageModel> landingPageCommand)
        {
            _mySqlSession = mySqlSession;
            _landingPageCommand = landingPageCommand;

            // chama o método para estabelecer a conexão com o banco de dados
            AbrirConexao();
        }

        #endregion

        #region métodos
        
        /// <summary>
        /// estabelece a conexão com o banco de dados
        /// </summary>
        private void AbrirConexao()
        {
            // condição para estabelecer a conexão com o banco de dados
            if (_mySqlSession._connection.State == System.Data.ConnectionState.Closed)
                _mySqlSession._connection.Open();
        }

        /// <summary>
        /// iniciar uma transação com o banco de dados
        /// </summary>
        public void BeginTransaction()
        {            
            _mySqlSession._transaction = _mySqlSession._connection.BeginTransaction();
        }

        /// <summary>
        /// comitar uma transação com o banco de dados
        /// </summary>
        public void Commit()
        {
            _mySqlSession._transaction.Commit();
        }

        /// <summary>
        /// desfazer uma transação com o banco de dados
        /// </summary>
        public void Rollback() 
        {
            _mySqlSession._transaction.Rollback();
        }

        public void Dispose() => _mySqlSession._transaction?.Dispose();

        #endregion

        #region propriedades

        public ICommand<LandingPageModel> LandingPageCommand => _landingPageCommand;

        #endregion

        #region destrutor

        ~UoW() 
        {
            Dispose();
        }

        #endregion
    }
}
