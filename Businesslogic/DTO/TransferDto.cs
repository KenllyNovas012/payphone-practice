
namespace Businesslogic.DTO
{
    public class TransferDto
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public short TransferType { get; set; }
        public DateTime CreatedAt { get; set; }
        public WalletDto Wallet { get; set; } = null!;
    }
}
