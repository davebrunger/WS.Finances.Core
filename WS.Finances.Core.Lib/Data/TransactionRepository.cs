using System.Collections.Generic;
using System.IO;
using System.Linq;
using WS.Finances.Core.Lib.Models;

namespace WS.Finances.Core.Lib.Data
{
    public class TransactionRepository : IFilterableTransactionRepository
    {
        public string BaseDirectory { get; private set; }

        public TransactionRepository(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
        }

        public IQueryable<Transaction> Get()
        {
            return FilteredGet();
        }

        public IQueryable<Transaction> FilteredGet(int? year = null, int? month = null, string accountName = null)
        {
            return GetAllJsonRepositories(year, month, accountName).SelectMany(a => a.Get()).AsQueryable();
        }

        public Transaction Get(Transaction key)
        {
            return GetJsonRepository(key).Get(key);
        }

        public bool Put(Transaction item)
        {
            return GetJsonRepository(item).Put(item);
        }

        public bool Delete(Transaction key)
        {
            return GetJsonRepository(key).Delete(key);
        }

        public void Clear()
        {
            foreach (var account in GetAllJsonRepositories(null, null, null))
            {
                account.Clear();
            }
        }

        private JsonRepository<Transaction> GetJsonRepository(Transaction key)
        {
            return new JsonRepository<Transaction>(Path.Combine(BaseDirectory, $@"{key.Year}-{key.Month}-{key.AccountName}.json"));
        }

        private IEnumerable<JsonRepository<Transaction>> GetAllJsonRepositories(int? year, int? month, string accountName)
        {
            var accountNamePattern = string.IsNullOrEmpty(accountName) ? "*" : accountName;
            var filePattern = $"{year?.ToString() ?? "*"}-{month?.ToString() ?? "*"}-{accountNamePattern}.json";
            return Directory.EnumerateFiles(BaseDirectory, filePattern).Select(f => new JsonRepository<Transaction>(f));
        }
    }
}
