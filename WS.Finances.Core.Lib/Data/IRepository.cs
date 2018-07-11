using System;
using System.Collections.Generic;
using System.Linq;

namespace WS.Finances.Core.Lib.Data
{
    public interface IRepository<TModel> 
        where TModel : class, IModel
    {
        IQueryable<TModel> Get();

        TModel Get(TModel key);

        bool Put(TModel item);

        bool Delete(TModel key);

        void Clear();
    }
}
