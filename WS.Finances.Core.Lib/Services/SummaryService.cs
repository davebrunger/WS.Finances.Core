using System.Collections.Generic;
using System.Linq;
using WS.Finances.Core.Lib.Models;

namespace WS.Finances.Core.Lib.Services
{
    public class SummaryService
    {
        private readonly TransactionService _transactionService;
        private readonly MapService _mapService;

        public SummaryService(TransactionService transactionService, MapService mapService)
        {
            _transactionService = transactionService;
            _mapService = mapService;
        }

        public IEnumerable<KeyValuePair<Map, decimal?>> Get(int year, int month)
        {
            var map = _mapService.Get();
            var transactions = _transactionService.Get(year, month).ToList();

            return map
                .GroupJoin(transactions, m => m.Category, t => t.Category,
                    (m, ts) => new { map = m, transactions = ts.ToList() })
                .Select(m =>
                {
                    decimal? sum;
                    if (m.transactions.Count == 0)
                    {
                        sum = null;
                    }
                    else
                    {
                        sum = m.transactions.Select(t => (t.MoneyIn ?? 0) - (t.MoneyOut ?? 0)).Sum();
                    }
                    return new KeyValuePair<Map, decimal?>(m.map, sum);
                });
        }

        public IEnumerable<KeyValuePair<Map, IEnumerable<decimal?>>> Get(int year)
        {
            var map = _mapService.Get();
            var transactions = _transactionService.Get(year).ToList();
            var months = Enumerable.Range(1, 12).ToList();

            return map
                .GroupJoin(transactions, m => m.Category, t => t.Category,
                    (m, ts) => new { map = m, transactions = ts.ToList() })
                .Select(m =>
                {
                    var monthSums = months
                        .GroupJoin(m.transactions, mo => mo, t => t?.Month ?? -1, (mo, ts) => new {month = mo, transactions = ts.ToList()});
                    var sums = monthSums
                        .OrderBy(mo => mo.month)
                        .Select(mo =>
                        {
                            if (mo.transactions.Count == 0 || (mo.transactions[0] == null))
                            {
                                return null;
                            }
                            return (decimal?)mo.transactions.Select(t => (t.MoneyIn ?? 0) - (t.MoneyOut ?? 0)).Sum();
                        }).ToList();
                    return new KeyValuePair<Map, IEnumerable<decimal?>>(m.map, sums);
                });
        }
    }
}
