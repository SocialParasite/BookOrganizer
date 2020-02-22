using System.IO;
using System.Reflection;
using Microsoft.Win32;

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
            => $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)}\\placeholder.png";

    }
}
