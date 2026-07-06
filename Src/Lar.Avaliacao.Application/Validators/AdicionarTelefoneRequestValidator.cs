using FluentValidation;
using Lar.Avaliacao.Application.Requests;

namespace Lar.Avaliacao.Application.Validators
{
    public class AdicionarTelefoneRequestValidator : AbstractValidator<AdicionarTelefoneRequest>
    {
        public AdicionarTelefoneRequestValidator()
        {
            RuleFor(x => x.Tipo)
                .IsInEnum().WithMessage("Tipo de telefone inválido. Use Celular, Residencial ou Comercial.");

            RuleFor(x => x.Numero)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Número é obrigatório.")
                .Must(numero => numero.All(char.IsDigit)).WithMessage("Número deve conter apenas dígitos.")
                .Length(10, 11).WithMessage("Número deve ter entre 10 e 11 dígitos.");
        }
    }
}
