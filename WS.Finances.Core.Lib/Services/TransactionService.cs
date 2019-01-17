using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WS.Finances.Core.Lib.Data;
using WS.Finances.Core.Lib.Models;

namespace WS.Finances.Core.Lib.Services
{
    public class TransactionService
    {
        private readonly RepositoryFactory _repositoryFactory;

        public TransactionService(RepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
            {
                throw new ArgumentNullException(nameof(repositoryFactory));
            }
            _repositoryFactory = repositoryFactory;
        }

        public IEnumerable<Transaction> Get(int? year = null, int? month = null, string accountName = null, string category = null,
            string descriptionPattern = null, bool unMappedOnly = false, IReadOnlyCollection<long> transactionIds = null, bool descriptionPatternIgnoreCase = false)
        {
            var repository = _repositoryFactory.GetRepository<Transaction>();
            var filterableRepository = repository as IFilterableTransactionRepository;
            IQueryable<Transaction> transactions;
            if (filterableRepository != null)
            {
                transactions = filterableRepository.FilteredGet(year, month, accountName);
            }
            else
            {
                transactions = repository.Get();
                if (year.HasValue)
                {
                    transactions = transactions.Where(t => t.Year == year.Value);
                }
                if (month.HasValue)
                {
                    transactions = transactions.Where(t => t.Month == month.Value);
                }
                if (!string.IsNullOrEmpty(accountName))
                {
                    transactions = transactions.Where(t => t.AccountName == accountName);
                }
            }
            if (!string.IsNullOrEmpty(category))
            {
                transactions = transactions.Where(t => t.Category == category);
            }
            if (!string.IsNullOrEmpty(descriptionPattern))
            {
                var descriptionRegex = new Regex(descriptionPattern,
                    descriptionPatternIgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                transactions = transactions.Where(t => descriptionRegex.IsMatch(t.Description));
            }
            if (unMappedOnly)
            {
                transactions = transactions.Where(t => string.IsNullOrEmpty(t.Category));
            }
            if (transactionIds?.Count > 0)
            {
                transactions = transactions.Where(t => transactionIds.Contains(t.TransactionID));
            }
            return transactions;
        }

        public void Clear(int? year = null, int? month = null, string accountName = null)
        {
            var repository = _repositoryFactory.GetRepository<Transaction>();
            foreach (var transaction in Get(year, month, accountName))
            {
                repository.Delete(transaction);
            }
        }

        public void Put(IEnumerable<Transaction> transactions)
        {
            var repository = _repositoryFactory.GetRepository<Transaction>();
            foreach (var transaction in transactions)
            {
                repository.Put(transaction);
            }
        }

        public void Put(params Transaction[] transactions)
        {
            Put(transactions.AsEnumerable());
        }
    }
}
