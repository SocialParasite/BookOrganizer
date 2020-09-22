using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using NetVips;

namespace BookOrganizer.UI.WPFCore.Services
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

            return (op.ShowDialog() == true) ? op.FileName : null;
        }

        public static string GetImagePath()
            => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)?.Substring(6)}\\placeholder.png";

        public static void CreateThumbnail(string path)
        {
            Image image = Image.Thumbnail(path, 75, 75);

            var newPath = "";
            int index = path.IndexOf(".", StringComparison.InvariantCulture);
            if (index > 0)
                newPath = path.Substring(0, index) + "_thumb.jpg";

            image.WriteToFile("test.jpg");
            File.Move("test.jpg", newPath);
        }
    }
}
