using Lar.Avaliacao.Application.Interfaces;

namespace Lar.Avaliacao.Infra.Security
{
    public class SystemClock :IClock
    {
        public DateTime Hoje => DateTime.Today;
    }
}
