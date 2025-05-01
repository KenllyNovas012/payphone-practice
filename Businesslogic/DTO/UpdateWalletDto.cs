namespace Businesslogic.DTO
{
    public class UpdateWalletDto
    {
        public string DocumentId { get; set; } = string.Empty;
        public string WalletName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
