using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
                var picture = new BitmapImage(new Uri(op.FileName));
                // TODO: testing...
                var picturePath = @"C:\\temp\\";

                return picturePath + picture.UriSource.Segments.LastOrDefault();
            }
            return null;
        }

        public static string GetImagePath()
            => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\placeholder.png";

    }
}
