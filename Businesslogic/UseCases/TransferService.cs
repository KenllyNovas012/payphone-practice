namespace Businesslogic.UseCases
{
    using Businesslogic.Interfaces;
    using Domain.Entities;
    using Domain.Enums;

    public class TransferService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransferRepository _transferRepository;

        public TransferService(IWalletRepository walletRepository, ITransferRepository transferRepository)
        {
            _walletRepository = walletRepository;
            _transferRepository = transferRepository;
        }

        public async Task TransferAsync(int fromId, int toId, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("El monto debe ser mayor que cero.");

            var fromWallet = await _walletRepository.GetByIdAsync(fromId) ?? throw new ArgumentException("Billetera origen no encontrada.");
            var toWallet = await _walletRepository.GetByIdAsync(toId) ?? throw new ArgumentException("Billetera destino no encontrada.");

            if (fromWallet.Balance < amount)
                throw new InvalidOperationException("Fondos insuficientes en la billetera de origen.");

            fromWallet.Balance -= amount;
            toWallet.Balance += amount;

            fromWallet.UpdatedAt = DateTime.UtcNow;
            toWallet.UpdatedAt = DateTime.UtcNow;

            await _walletRepository.UpdateAsync(fromWallet);
            await _walletRepository.UpdateAsync(toWallet);

            var txOut = new TransferHistory
            {
                WalletId = fromId,
                Amount = amount,
                TransferType = (short)Enums.TransferType.Out,
                CreatedAt = DateTime.UtcNow
            };

            var txIn = new TransferHistory
            {
                WalletId = toId,
                Amount = amount,
                TransferType = (short)Enums.TransferType.In,
                CreatedAt = DateTime.UtcNow
            };

            await _transferRepository.AddAsync(txOut);
            await _transferRepository.AddAsync(txIn);
        }
    }
}
