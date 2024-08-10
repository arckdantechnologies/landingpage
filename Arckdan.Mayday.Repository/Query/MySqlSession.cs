using Arckdan.Mayday.Repository.Context;
using MySqlConnector;
using System.ComponentModel;
using System.Data;

namespace Arckdan.Mayday.Repository.Query
{
    public sealed class MySqlSession : IDisposable
    {
        #region atributos

        private readonly bool _disposed = false;
        private readonly IntPtr _handle;
        public IDbConnection _connection;
        public IDbTransaction _transaction;

        #endregion

        #region construtores

        public MySqlSession(MaydayMySqlContext context) 
        {
            _connection = new MySqlConnection(context.MysqlConnectionString);
        }

        #endregion

        #region métodos

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern Boolean CloseHandle(IntPtr handle);

        /// <summary>
        /// método utilizado para encerrar a conexão com o banco de dados
        /// </summary>
        private void EncerrarConexao()
        {
            // remove os recursos gerenciáveis e não gerenciáveis na memória
            Dispose(disposing: false);
        }

        /// <summary>
        /// método utilizado para dispensar os recursos presos na memória
        /// </summary>
        public void Dispose()
        {
            // chama o dispose para forçar a limpeza do heap
            Dispose(disposing: true);

            // força a passagem do garbage collector
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// método utilizado para controlar a limpeza dos recursos em memória
        /// </summary>
        public void Dispose(bool disposing)
        {
            // bloco de construção de objetos
            Component _component = new Component();

            // verifica se o dispose já foi chamado
            if (!_disposed)
            {
                // condição para o dispose de recursos gerenciados e não gerenciados
                if (disposing)
                    _component.Dispose();

                // chama os métodos apropriados para limpeza dos recursos não gerenciados
                CloseHandle(_handle);
            }
        }

        #endregion

        #region destrutor

        /// <summary>
        /// destrutor da classe Connection
        /// </summary>
        ~MySqlSession()
        {
            // força a passagem do gargabe collector
            EncerrarConexao();
        }

        #endregion
    }
}