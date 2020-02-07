using System;
using System.IO;

namespace BookOrganizer.Domain
{
    public static class DomainHelpers
    {
        public static string SetPicturePath(string path, string subDir)
        {
            var pictureName = Path.GetFileName(path);

            var envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            var folder = $@"\BookOrganizer\{subDir}\";
            var fullPath = $"{envPath}{folder}{pictureName}";

            fullPath = Path.GetFullPath(fullPath);

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                if (File.Exists(path) && !File.Exists(Path.GetFullPath(fullPath)))
                    File.Copy(path, fullPath);

                return fullPath;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
