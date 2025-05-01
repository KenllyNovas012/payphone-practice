using Domain.Entities;

namespace Businesslogic.Interfaces
{
    public interface ITransferRepository
    {
        Task<IEnumerable<TransferHistory>> GetAllAsync();
        Task AddAsync(TransferHistory tx);
    }
}
