using FluentValidation;
using Lar.Avaliacao.Application.Requests;
using System.Text.RegularExpressions;

namespace Lar.Avaliacao.Application.Validators
{
    public partial class AdicionarEnderecoRequestValidator : AbstractValidator<AdicionarEnderecoRequest>
    {
        public AdicionarEnderecoRequestValidator()
        {
            RuleFor(x => x.Rua)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Rua é obrigatória.")
                .Length(3, 250).WithMessage("Rua deve ter entre 3 e 250 caracteres.");

            RuleFor(x => x.Numero)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Número é obrigatório.")
                .MaximumLength(10).WithMessage("Número deve ter no máximo 10 caracteres.");

            RuleFor(x => x.Complemento)
                .MaximumLength(250).WithMessage("Complemento deve ter no máximo 250 caracteres.");

            RuleFor(x => x.Referencia)
                .MaximumLength(250).WithMessage("Referência deve ter no máximo 250 caracteres.");

            RuleFor(x => x.Cidade)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Cidade é obrigatória.")
                .MaximumLength(150).WithMessage("Cidade deve ter no máximo 150 caracteres.");

            RuleFor(x => x.Estado)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Estado é obrigatório.")
                .Length(2).WithMessage("Estado deve ter exatamente 2 caracteres.")
                .Matches(SiglaEstadoRegex()).WithMessage("Estado deve conter apenas letras (ex: PR, SP).");
        }

        [GeneratedRegex("^[A-Za-z]{2}$")]
        private static partial Regex SiglaEstadoRegex();
    }
}
