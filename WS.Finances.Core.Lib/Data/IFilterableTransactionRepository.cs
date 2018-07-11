using System.Linq;
using WS.Finances.Core.Lib.Models;

namespace WS.Finances.Core.Lib.Data
{
    public interface IFilterableTransactionRepository : IRepository<Transaction>
    {
        IQueryable<Transaction> FilteredGet(int? year = null, int? month = null, string accountName = null);
    }
}
