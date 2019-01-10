using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace WS.Finances.Core.Lib.Data
{
    public class JsonRepository<TModel> : IRepository<TModel>
        where TModel : class, IModel
    {
        private List<TModel> _data;

        public string FileName { get; }

        public JsonRepository(string fileName)
        {
            FileName = fileName;
        }

        public IQueryable<TModel> Get()
        {
            LoadData();
            return Copy(_data).AsQueryable();
        }

        public TModel Get(TModel key)
        {
            LoadData();
            var result = _data.SingleOrDefault(i => i.IsIdentifierEqualTo(key));
            return result == null ? null : Copy(result);
        }

        public bool Put(TModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var result = Delete(item, false);
            _data.Add(Copy(item));
            SaveData();
            return result;
        }

        public bool Delete(TModel key)
        {
            return Delete(key, true);
        }

        private bool Delete(TModel key, bool saveData)
        {
            LoadData();
            var oldItem = _data.SingleOrDefault(i => i.IsIdentifierEqualTo(key));
            if (oldItem == null)
            {
                return false;
            }
            _data.Remove(oldItem);
            if (saveData)
            {
                SaveData();
            }
            return true;
        }

        public void Clear()
        {
            _data = new List<TModel>();
            SaveData();
        }

        private void SaveData()
        {
            if (_data != null)
            {
                var data = JsonConvert.SerializeObject(_data, Formatting.Indented);
                var path = Path.GetDirectoryName(FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllText(FileName, data);
            }
        }

        private void LoadData()
        {
            if (_data == null)
            {
                if (File.Exists(FileName))
                {
                    var data = File.ReadAllText(FileName);
                    _data = JsonConvert.DeserializeObject<IEnumerable<TModel>>(data).ToList();
                }
                else
                {
                    _data = new List<TModel>();
                }
            }
        }

        private static T Copy<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }
    }
}
