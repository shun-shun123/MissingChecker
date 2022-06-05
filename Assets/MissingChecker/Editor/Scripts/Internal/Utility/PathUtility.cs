using System.IO;
using UnityEngine;

namespace MissingChecker
{
    internal static class PathUtility 
    {

        private const string MISSING_CHECKER = "MissingChecker";

        internal const string PRESET = "Presets";

        internal static string PresetDirectory => Combine(PRESET);

        internal static string Combine(string filePath)
        {
            return $"{Application.dataPath}/../{MISSING_CHECKER}/{filePath}";
        }

        internal static DirectoryInfo GetDirectoryInfo(string directory)
        {
            directory = directory.ReplaceBackSlash();
            if (Directory.Exists(directory))
            {
                return new DirectoryInfo(directory);
            }
            return null;
        }

        internal static string GetDirectoryName(string path)
        {
            var directory = Path.GetDirectoryName(path.ReplaceBackSlash());
            directory = directory.ReplaceBackSlash();
            return directory;
        }

        internal static string GetFileName(string path)
        {
            path = path.ReplaceBackSlash();
            return Path.GetFileName(path);
        }

        private static string ReplaceBackSlash(this string target)
        {
            return target.Replace("\\", "/");
        }
    }
}
