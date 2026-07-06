using FluentValidation;
using Lar.Avaliacao.Application.Requests;

namespace Lar.Avaliacao.Application.Validators
{
    public class RegistrarUsuarioRequestValidator : AbstractValidator<RegistrarUsuarioRequest>
    {
        public RegistrarUsuarioRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(250).WithMessage("Nome deve ter no máximo 250 caracteres.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email possui formato inválido.")
                .MaximumLength(250).WithMessage("Email deve ter no máximo 250 caracteres.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.")
                .Matches("[A-Za-z]").WithMessage("Senha deve conter ao menos uma letra.")
                .Matches("[0-9]").WithMessage("Senha deve conter ao menos um número.");
        }
    }
}
