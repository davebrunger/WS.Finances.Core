using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WS.Finances.Core.Lib.Data;
using WS.Utilities.Csv;

namespace WS.Finances.Core.Lib.Models
{
    public class Account : IModel
    {
        public string Name { get; set; }

        public int StartRow { get; set; }

        public int TimestampColumn { get; set; }

        public string TimestampFormat { get; set; }

        public int DescriptionColumn { get; set; }

        public int? MoneyInColumn { get; set; }

        public int? MoneyOutColumn { get; set; }

        public int? TotalColumn { get; set; }

        public int GetMaxColumnIndex() => (new[] { TimestampColumn, DescriptionColumn, MoneyInColumn ?? 0, MoneyOutColumn ?? 0, TotalColumn ?? 0 }).Max();

        public Func<int, IReadOnlyList<string>, Option<Transaction>> GetTransactionParser(int year, int month, string sourceFileName, Func<string, string> mapDescriptionToCategory)
        {
            return (rowNumber, rowContents) =>
            {
                if ((rowNumber < StartRow) ||
                    (rowContents.Count <= GetMaxColumnIndex()) ||
                    string.IsNullOrEmpty(rowContents[DescriptionColumn]) ||
                    string.IsNullOrEmpty(rowContents[TimestampColumn]))
                {
                    return Option<Transaction>.None;
                }
                return Option<Transaction>.Some(new Transaction()
                {
                    AccountName = Name,
                    Balance = ParseAmount(rowContents, TotalColumn),
                    Category = mapDescriptionToCategory(rowContents[DescriptionColumn]),
                    Description = rowContents[DescriptionColumn],
                    MoneyIn = ParseAmount(rowContents, MoneyInColumn),
                    MoneyOut = ParseAmount(rowContents, MoneyOutColumn),
                    Month = month,
                    SourceFileName = sourceFileName,
                    Timestamp = ParseTimestamp(rowContents, TimestampColumn, TimestampFormat),
                    TransactionID = rowNumber,
                    Year = year
                });
            };
        }

        private static DateTime ParseTimestamp(IReadOnlyList<string> rowContents, int columnIndex, string format)
        {
            try
            {
                return DateTime.ParseExact(rowContents[columnIndex].Trim(), format, CultureInfo.InvariantCulture);
            }
            catch (Exception exception)
            {
                throw new Exception($"Error parsing timestamp {rowContents[columnIndex]} with format {format}", exception);
            }
        }

        private static decimal? ParseAmount(IReadOnlyList<string> rowContents, int? columnIndex)
        {
            if ((columnIndex.HasValue) && (!string.IsNullOrEmpty(rowContents[columnIndex.Value])))
            {
                var amount = rowContents[columnIndex.Value]
                    .Replace("\uFFFD", "")
                    .Replace("£", "")
                    .Replace("–", "-").Trim();
                var sign = 1;
                if (amount.StartsWith("(") && amount.EndsWith(")"))
                {
                    amount = amount
                        .Replace("(", "")
                        .Replace(")", "");
                    sign = -1;
                }
                return amount.Length == 0 ? (decimal?)null : decimal.Parse(amount) * sign;
            }
            return null;
        }

        public bool IsIdentifierEqualTo(IModel model)
        {
            var other = model as Account;
            return other != null && Name == other.Name;
        }
    }
}
