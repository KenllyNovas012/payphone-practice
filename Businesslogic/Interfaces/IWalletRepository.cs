using Domain.Entities;

namespace Businesslogic.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetByIdAsync(int id);
        Task<Wallet?> GetByDocumentIdAsync(string documentId);
        Task<IEnumerable<Wallet>> GetAllAsync();
        Task AddAsync(Wallet wallet);
        Task UpdateAsync(Wallet wallet);
        Task DeleteAsync(int id);
    }
}
