using Arckdan.Mayday.Domain.Comunidade;
using Arckdan.Mayday.Repository.Context;
using Arckdan.Mayday.Repository.Interface;
using Dapper;
using System.Data;
using System.Text;
using MySqlConnector;
using Arckdan.Mayday.Services.Mensagem.Models.Sistema;
using Arckdan.Mayday.Services.Mensagem.Enums;

namespace Arckdan.Mayday.Repository.Query.Comunidade
{
    public class LandingPageQuery : IQuery
    {
        #region atributos

        private readonly IDbConnection _mySqlConnection;

        #endregion

        #region construtores

        /// <summary>
        /// construtor da classe LandingPageQuery
        /// </summary>
        /// <param name="context">context para a conexão com o banco MySql</param>
        public LandingPageQuery(MaydayMySqlContext context)
        {
            _mySqlConnection = new MySqlSession(context)._connection;
        }

        #endregion

        #region métodos

        /// <summary>
        /// obter a listagem de todos os early adopters
        /// </summary>
        /// <returns>retorna a lista com os dados dos early adopters</returns>
        public Retorno Listar()
        {
            // bloco de declaração de variáveis
            var query = _mySqlConnection.Query<LandingPageModel>($"{Query}");

            // bloco de tratamento de exceção
            try
            {
                var registro = new Registro<LandingPageModel>(ERetorno.Sucesso, string.Empty, query);

                // condição para tratar os registros não encontrados
                if (query.Count().Equals(0))
                    registro = new Registro<LandingPageModel>(ERetorno.Alerta, string.Empty);

                return registro;
            }
            catch (MySqlException ex)
            {
                return new Registro<LandingPageModel>(ERetorno.Erro, ex.Message);
            }
            catch (Exception ex)
            {
                return new Registro<LandingPageModel>(ERetorno.Erro, ex.Message);
            }
        }

        /// <summary>
        /// obter a listagem dos early adopters com condição de pesquisa
        /// </summary>
        /// <param name="where">condiçãoo de pesquisa usada na listagem</param>
        /// <returns>retorna a lista com os dados dos early adopters a partir da condição informada na pesquisa</returns>
        public Retorno Listar(string? where)
        {
            // bloco de declaração de variáveis
            var sql = $"{Query} where {where}";
            var query = _mySqlConnection.Query<LandingPageModel>(sql);

            // bloco de tratamento de exceção
            try
            {
                var registro = new Registro<LandingPageModel>(ERetorno.Sucesso, string.Empty, query);

                // condição para tratar os registros não encontrados
                if (query.Count().Equals(0))
                    registro = new Registro<LandingPageModel>(ERetorno.Alerta, string.Empty);

                return registro;
            }
            catch (MySqlException ex)
            {
                return new Registro<LandingPageModel>(ERetorno.Erro, ex.Message);
            }
            catch (Exception ex)
            {
                return new Registro<LandingPageModel>(ERetorno.Erro, ex.Message);
            }
        }

        /// <summary>
        /// obter o registro do early adopter
        /// </summary>
        /// <param name="id">código do registro para pesquisa</param>
        /// <returns>retorna o registro do early adopter pesquisado</returns>
        public Retorno Obter(Guid id)
        {
            {
                // bloco de declaração de variáveis;
                var query = _mySqlConnection.Query<LandingPageModel>($"{Query} where Id = '{id}'", new { Id = id });

                // bloco de tratamento de exceção
                try
                {
                    var registro = new Registro<LandingPageModel>(ERetorno.Sucesso, string.Empty, query);

                    // condição para tratar os registros não encontrados
                    if (query.Count().Equals(0))
                        registro = new Registro<LandingPageModel>(ERetorno.Alerta, string.Empty);

                    return registro;
                }
                catch (MySqlException ex)
                {
                    return new Registro<LandingPageModel>(ERetorno.Erro, ex.Message);
                }
                catch (Exception ex)
                {
                    return new Registro<LandingPageModel>(ERetorno.Erro, ex.Message);
                }
            }
        }

        #endregion

        #region propriedades

        private StringBuilder Query => new StringBuilder()
            .Append(" select Id ")
            .Append(", Ip ")
            .Append(", Nome")
            .Append(", UF")
            .Append(", Cidade ")
            .Append(", Email ")
            .Append(", WhatsApp ")
            .Append(", Token ")
            .Append(", Inclusao")
            .Append(", Alteracao")
            .Append(" from tb_mayday_landing_page ");

        #endregion
    }
}