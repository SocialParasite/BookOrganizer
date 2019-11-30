using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Extensions
{
    public static class CollectionExtensions
    {
        public static List<T> FromListToList<T>(this IEnumerable<T> enumerableList)
        {
            return enumerableList != null ? new List<T>(enumerableList) : null;
        }
    }

}
