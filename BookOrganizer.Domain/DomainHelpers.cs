using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                if (Directory.Exists(Path.GetDirectoryName(fullPath))) return fullPath;

                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }
            catch (Exception ex)
            {
                throw; //TODO:
            }

            return fullPath;
        }
    }
}
