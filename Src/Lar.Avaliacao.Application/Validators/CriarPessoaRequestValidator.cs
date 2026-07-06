using FluentValidation;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Validators
{
    public class CriarPessoaRequestValidator : AbstractValidator<CriarPessoaRequest>
    {
        public CriarPessoaRequestValidator(IClock clock)
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(250).WithMessage("Nome deve ter no máximo 250 caracteres.");

            RuleFor(x => x.Cpf)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Must(cpf => cpf.All(char.IsDigit)).WithMessage("CPF deve conter apenas dígitos.")
                .Length(11).WithMessage("CPF deve ter exatamente 11 dígitos.");

            RuleFor(x => x.DataNascimento)
                .NotEqual(default(DateTime)).WithMessage("Data de nascimento é obrigatória.")
                .LessThan(clock.Hoje).WithMessage("Data de nascimento deve ser uma data passada.");
        }
    }
}
