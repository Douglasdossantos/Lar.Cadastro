using Lar.Avaliacao.Application.Interfaces;

namespace Lar.Avaliacao.Tests.TestHelpers
{
    public class FakeClock : IClock
    {
        public DateTime Hoje { get; }

        public FakeClock(DateTime hoje)
        {
            Hoje = hoje.Date;
        }

        public static FakeClock Em(int ano, int mes, int dia) => new(new DateTime(ano, mes, dia));
    }
}
