using System;
using System.Text;
using System.IO;

namespace Shaman.Runtime
{
#if !STANDALONE
    [RestrictedAccess]
#endif
    /// <summary>
    /// File system related methods.
    /// </summary>
    public static class FileSystem
    {

        private static string SanitizeFileName(string folder, string name, int index)
        {
            const int MaxPathLength = 200;

            var sb = new StringBuilder(name);
            foreach (var item in System.IO.Path.GetInvalidFileNameChars())
            {
                sb.Replace(item, '-');
            }
            name = sb.ToString();

            var extension = Path.GetExtension(name).Trim();
            var baseName = Path.GetFileNameWithoutExtension(name).Trim();
            if (string.IsNullOrEmpty(baseName)) baseName = "Untitled";
            var indexString = index == 1 ? string.Empty : string.Format(" ({0})", index);

            name = baseName + indexString + extension;

            if (folder.Length + name.Length + 1 > MaxPathLength)
            {
                name = baseName.Substring(0, MaxPathLength - indexString.Length - folder.Length - 1 - extension.Length) + indexString + extension;
            }

            return Path.Combine(folder, name);
        }

        public static string SanitizeFileName(string folder, string name)
        {
            return SanitizeFileName(folder, name, 1);
        }

        public static string GetUniqueFileName(string folder, string name)
        {
            for (int i = 1; ; i++)
            {
                var path = SanitizeFileName(folder, name, i);
                if (!File.Exists(path)) return path;
            }
        }



        public static void DeleteOrMoveFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (DirectoryNotFoundException)
            {
                // it's ok
            }
            catch (UnauthorizedAccessException ex1)
            {
                try
                {
                    File.Move(path, Path.Combine(Path.GetDirectoryName(path), "$DELETE_" + Guid.NewGuid() + ".tmp"));
                }
                catch
                {
                    throw;
                }
            }
        }

    }




}
