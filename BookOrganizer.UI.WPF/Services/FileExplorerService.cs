using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace BookOrganizer.UI.WPF.Services
{
    public static class FileExplorerService
    {
        public static string BrowsePicture()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an image as an author picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                var coverPic = new BitmapImage(new Uri(op.FileName));
                // TODO: testing...
                var coverPath = @"C:\\temp\\";

                return coverPath + coverPic.UriSource.Segments.LastOrDefault();
            }
            return null;
        }
    }
}
