using System.Collections.Generic;
using WS.Finances.Core.Lib.Data;

namespace WS.Finances.Core.Lib.Models
{
    public class Map : IModel
    {
        public string Category { get; set; }

        public string Section { get; set; }

        public IEnumerable<string> Patterns { get; set; }

        public int Position { get; set; }

        public bool IsIdentifierEqualTo(IModel model)
        {
            var other = model as Map;
            return other != null && Category == other.Category;
        }
    }
}
