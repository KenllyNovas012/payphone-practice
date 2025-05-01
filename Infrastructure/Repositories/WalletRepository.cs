using Businesslogic.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _context;

        public WalletRepository(WalletDbContext context) => _context = context;

        public async Task AddAsync(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet != null)
            {
                _context.Wallets.Remove(wallet);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Wallet>> GetAllAsync() => await _context.Wallets.ToListAsync();

        public async Task<Wallet?> GetByDocumentIdAsync(string documentId) => await _context.Wallets.Where(x => x.DocumentId == documentId).FirstOrDefaultAsync();


        public async Task<Wallet?> GetByIdAsync(int id) => await _context.Wallets.FindAsync(id);

        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }
    }
}
