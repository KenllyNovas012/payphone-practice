namespace Businesslogic.DTO
{
    public class TransferRequestDto
    {
        public int FromWalletId { get; set; }
        public int ToWalletId { get; set; }
        public decimal Amount { get; set; }
    }
}
