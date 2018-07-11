using System;
using System.Collections.Generic;
using System.Linq;
using WS.Finances.Core.Lib.Data;
using WS.Finances.Core.Lib.Models;

namespace WS.Finances.Core.Lib.Services
{
    public class MapService
    {
        private readonly RepositoryFactory _repositoryFactory;

        public MapService(RepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
            {
                throw new ArgumentNullException(nameof(repositoryFactory));
            }
            _repositoryFactory = repositoryFactory;
        }

        public IEnumerable<Map> Get()
        {
            var mapRepository = _repositoryFactory.GetRepository<Map>();
            return mapRepository.Get().ToList();
        }
        public Map Get(string category)
        {
            var mapRepository = _repositoryFactory.GetRepository<Map>();
            return mapRepository.Get(new Map { Category = category });
        }
    }
}
