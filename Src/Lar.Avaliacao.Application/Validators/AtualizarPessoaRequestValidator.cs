using FluentValidation;
using Lar.Avaliacao.Application.Requests;

namespace Lar.Avaliacao.Application.Validators
{
    public class AtualizarPessoaRequestValidator : AbstractValidator<AtualizarPessoaRequest>
    {
        public AtualizarPessoaRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(250).WithMessage("Nome deve ter no máximo 250 caracteres.");
        }
    }
}
