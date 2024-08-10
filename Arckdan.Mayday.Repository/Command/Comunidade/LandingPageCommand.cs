using Arckdan.Mayday.Domain.Comunidade;
using Arckdan.Mayday.Repository.Context;
using Arckdan.Mayday.Repository.Interface;

namespace Arckdan.Mayday.Repository.Command.Comunidade
{
    public class LandingPageCommand : ICommand<LandingPageModel>
    {
        #region atributos

        private readonly MaydayMySqlContext _dbContext;

        #endregion

        #region construtores

        /// <summary>
        /// construtor da classe LandingPageCommand
        /// </summary>
        /// <param name="dbContext">contexto do banco de dados para conexão</param>
        public LandingPageCommand(MaydayMySqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region métodos

        /// <summary>
        /// incluir um novo registro
        /// </summary>
        /// <param name="p">entidade da call to action para a landing page</param>
        public void Incluir(LandingPageModel p)
        {
            _dbContext.Add(LandingPageModel.LandingPageFactory.ObterModel(p.Id, p.IP, p.Nome, p.UF, p.Cidade, p.Email, p.WhatsApp, p.Token, p.Inclusao, p.Alteracao));
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// alterar um registro
        /// </summary>
        /// <param name="p">entidade da call to action para a landing page</param>
        public void Alterar(LandingPageModel p)
        {
            _dbContext.Update(LandingPageModel.LandingPageFactory.ObterModel(p.Id, p.IP, p.Nome, p.UF, p.Cidade, p.Email, p.WhatsApp, p.Token, p.Inclusao, p.Alteracao));
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// excluir um registro
        /// </summary>
        /// <param name="id">código do registro a ser excluído da base de landing page</param>
        public void Excluir(Guid id)
        {
            _dbContext.Remove(LandingPageModel.LandingPageFactory.ObterModel(id));
            _dbContext.SaveChanges();
        }

        #endregion
    }
}