using BookOrganizer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.Web.Models
{
    public class BaseDetailViewModel<T>
    {
        public T SelectedItem { get; set; }

        public string GetItemPicture(string path) 
            => System.Net.WebUtility.UrlEncode(System.IO.Path.GetFileName(path));
    }
}
