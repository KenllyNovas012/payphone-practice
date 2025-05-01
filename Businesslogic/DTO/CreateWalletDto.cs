namespace Businesslogic.DTO
{
    public class WalletDto
    {
        public int Id { get; set; }
        public string DocumentId { get; set; } = string.Empty;
        public string WalletName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
