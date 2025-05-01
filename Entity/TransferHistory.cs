using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class TransferHistory
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public short TransferType { get; set; }
        public DateTime CreatedAt { get; set; }

        public Wallet Wallet { get; set; } = null!;
    }
}
