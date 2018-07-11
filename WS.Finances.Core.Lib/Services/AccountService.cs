using System;
using System.Collections.Generic;
using System.Linq;
using WS.Finances.Core.Lib.Data;
using WS.Finances.Core.Lib.Models;

namespace WS.Finances.Core.Lib.Services
{
    public class AccountService
    {
        private readonly RepositoryFactory _repositoryFactory;

        public AccountService(RepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
            {
                throw new ArgumentNullException(nameof(repositoryFactory));
            }
            _repositoryFactory = repositoryFactory;
        }

        public IEnumerable<Account> Get()
        {
            var accountRepository = _repositoryFactory.GetRepository<Account>();
            return accountRepository.Get().ToList();
        }

        public Account Get(string accountName)
        {
            var accountRepository = _repositoryFactory.GetRepository<Account>();
            return accountRepository.Get(new Account { Name = accountName });
        }
    }
}
