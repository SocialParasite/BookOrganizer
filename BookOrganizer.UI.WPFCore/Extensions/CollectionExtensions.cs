﻿using System.Collections.Generic;

namespace BookOrganizer.UI.WPFCore.Extensions
{
    public static class CollectionExtensions
    {
        public static List<T> FromListToList<T>(this IEnumerable<T> enumerableList)
        {
            return enumerableList != null ? new List<T>(enumerableList) : null;
        }
    }

}
