using System;
using WS.Finances.Core.Lib.Data;

namespace WS.Finances.Core.Lib.Models
{
    public class Transaction : IModel
    {
        public string AccountName { get; set; }

        // ReSharper disable once InconsistentNaming
        public long TransactionID { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string SourceFileName { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal? MoneyIn { get; set; }

        public decimal? MoneyOut { get; set; }

        public decimal? Balance { get; set; }

        public bool IsIdentifierEqualTo(IModel model)
        {
            var other = model as Transaction;
            if (other == null)
            {
                return false;
            }
            if (AccountName != other.AccountName)
            {
                return false;
            }
            if (TransactionID != other.TransactionID)
            {
                return false;
            }
            if (Year != other.Year)
            {
                return false;
            }
            if (Month != other.Month)
            {
                return false;
            }
            return true;
        }
    }
}
