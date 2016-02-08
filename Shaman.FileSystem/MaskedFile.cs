using System;
using System.IO;

namespace Shaman.Runtime
{



    /// <summary>
    /// Methods that provide transactional guarantees when creating files
    /// </summary>
#if !STANDALONE
    [RestrictedAccess]
#endif
    public static class MaskedFile
    {
        public static string GetMaskedPathFromFile(string file)
        {
            return GetMaskedPathFromFolder(Path.GetDirectoryName(file));
        }

        public static string GetMaskedPathFromFileWithCustomExtension(string file, string extension)
        {
            return Path.Combine(Path.GetDirectoryName(file), "$" + Guid.NewGuid().ToString() + extension);
        }

        public static string GetMaskedPathFromFolder(string folder)
        {
            return Path.Combine(folder, "$" + Guid.NewGuid().ToString() + ".tmp");
        }

        public static void PublishMaskedFile(string maskedPath, string finalName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(finalName));
            File.Delete(finalName);
            File.Move(maskedPath, finalName);
            SetHiddenAttribute(finalName, false);
        }

        public static void SetHiddenAttribute(string path, bool value)
        {
            FileAttributes fileAttributes = File.GetAttributes(path);
            if (value)
            {
                fileAttributes |= FileAttributes.Hidden;
            }
            else
            {
                fileAttributes &= ~FileAttributes.Hidden;
            }
            File.SetAttributes(path, fileAttributes);
        }

        public static void TryDeleteTempFile(string tempFile)
        {
            try
            {
                File.Delete(tempFile);
            }
            catch
            {
            }
        }



    }
}

