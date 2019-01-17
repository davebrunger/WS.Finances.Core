using System.Collections.Generic;

namespace WS.Finances.Core.Web.Models
{
    public class MapTransactionRequest
    {
        public string Pattern { get; set; }
        public IEnumerable<long> TransactionIds { get; set; }
        public string Category { get; set; }
    }
}
