using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Utils
{
    public static class UtilsExtension
    {
        public static IEnumerable<(T Item, int Index)> WithIndex<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
                yield return (list[i], i);
        }
    }
}
