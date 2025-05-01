using Businesslogic.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly WalletDbContext _context;

        public TransferRepository(WalletDbContext context) => _context = context;
        public async Task AddAsync(TransferHistory transfer)
        {
            _context.TransferHistories.Add(transfer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransferHistory>> GetAllAsync() => await _context.TransferHistories
            .Include(t => t.Wallet)
            .ToListAsync();
    }
}
