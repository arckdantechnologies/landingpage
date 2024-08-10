using Arckdan.Mayday.Domain.Comunidade;
using FluentValidation;

namespace Arckdan.Mayday.Services.Autenticacao
{
    public class ValidacaoLandingPageServico : AbstractValidator<LandingPageModel>
    {
        #region construtores

        /// <summary>
        /// construtor da classe ValidacaoLandingPageServico
        /// </summary>
        public ValidacaoLandingPageServico()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Id é obrigatório");
            RuleFor(x => x.IP).NotNull().WithMessage("Ip é obrigatório");
            RuleFor(x => x.Nome).NotNull().WithMessage("Nome é obrigatório");
            RuleFor(x => x.UF).NotNull().WithMessage("UF é obrigatório");
            RuleFor(x => x.Cidade).NotNull().WithMessage("Cidade é obrigatório");
            RuleFor(x => x.Email).NotNull().WithMessage("E-mail é obrigatório");
            RuleFor(x => x.WhatsApp).NotNull().WithMessage("Whatsapp é obrigatório");
            RuleFor(x => x.Inclusao).NotNull().WithMessage("Data de inclusão é obrigatório");
        }

        #endregion
    }
}
