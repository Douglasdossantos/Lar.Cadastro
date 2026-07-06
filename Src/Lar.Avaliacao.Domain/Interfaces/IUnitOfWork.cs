namespace Lar.Avaliacao.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SalvarAlteracoesAsync(CancellationToken ct = default);
    }
}