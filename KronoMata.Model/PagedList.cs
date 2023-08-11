using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoMata.Model
{
    [Serializable]
    public class PagedList<T>
    {
        public int TotalRecords { get; set; }
        public List<T> List { get; set; } = new List<T>();
    }
}
