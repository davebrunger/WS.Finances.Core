using System;
using System.IO;
using WS.Finances.Core.Lib.Models;

namespace WS.Finances.Core.Lib.Data
{
    public class RepositoryFactory
    {
        public IBaseDirectoryProvider BaseDirectoryProvider { get; }

        public RepositoryFactory(IBaseDirectoryProvider baseDirectoryProvider)
        {
            BaseDirectoryProvider = baseDirectoryProvider;
        }

        public IRepository<TModel> GetRepository<TModel>()
            where TModel : class, IModel
        {
            if (typeof(TModel) == typeof(Account))
            {
                return new JsonRepository<TModel>(Path.Combine(BaseDirectoryProvider.BaseDirectory, "Accounts.json"));
            }
            if (typeof(TModel) == typeof(Map))
            {
                return new JsonRepository<TModel>(Path.Combine(BaseDirectoryProvider.BaseDirectory, "Map.json"));
            }
            if (typeof(TModel) == typeof(Transaction))
            {
                return new TransactionRepository(BaseDirectoryProvider.BaseDirectory) as IRepository<TModel>;
            }
            throw new ArgumentException("Invalid type parameter combination");
        }
    }
}
