using Arckdan.Mayday.Domain.Comunidade;
using Microsoft.EntityFrameworkCore;

namespace Arckdan.Mayday.Repository.Context
{
    public class MaydayMySqlContext : DbContext
    {
        #region construtores

        /// <summary>
        /// construtor da classe MaydayMySqlContext
        /// </summary>
        /// <param name="options">opções de configuração da base de dados MySql</param>
        public MaydayMySqlContext(DbContextOptions<MaydayMySqlContext> options) : base(options) 
        {
            MysqlConnectionString = base.Database.GetConnectionString();
        }

        #endregion

        #region métodos

        /// <summary>
        /// criação dos modelos baseados em estruturas pré definidas com code first
        /// </summary>
        /// <param name="m">modelo de criação da base de dados MySql</param>
        protected override void OnModelCreating(ModelBuilder m)
        {
        }

        #endregion

        #region tabelas

        public DbSet<LandingPageModel> tb_mayday_landing_page { get; set; }

        #endregion

        #region propriedades

        public string? MysqlConnectionString { get; private set; }

        #endregion
    }
}
