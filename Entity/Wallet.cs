using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Wallet
    {
        public int Id { get; set; }
        public string DocumentId { get; set; } = string.Empty;
        public string WalletName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<TransferHistory> Transfers { get; set; } = new List<TransferHistory>();
    }
}
