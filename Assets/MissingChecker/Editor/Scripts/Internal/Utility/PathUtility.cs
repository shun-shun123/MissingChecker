using System.IO;
using UnityEngine;

namespace MissingChecker
{
    internal static class PathUtility 
    {

        private const string MISSING_CHECKER = "MissingChecker";

        internal const string PRESET = "Presets";

        internal static string PresetDirectory => Combine(PRESET);

        internal static (bool, string) ValidateCheckPath(string path)
        {
            if (path.EndsWith(" "))
            {
                return (false, "パス名の末尾に空白が含まれています");
            }

            if (!path.StartsWith("Assets/"))
            {
                return (false, "チェック対象はAssets/以下である必要があります");
            }

            if (!path.EndsWith("/"))
            {
                return (false, "チェック対象のパス名は\"/\"で終わる必要があります"); 
            }

            var invalidPathChars = Path.GetInvalidPathChars();
            foreach (var c in invalidPathChars)
            {
                if (path.Contains(c.ToString()))
                {
                    return (false, $"パス名では使えない{c}が含まれています");
                }
            }
            return (true, "");
        }

        internal static (bool, string) ValidateCheckExtension(string extension)
        {
            if (!extension.StartsWith("."))
            {
                return (false, "拡張子は\".\"から始まる必要があります");
            }

            if (extension.EndsWith(" "))
            {
                return (false, "拡張子の末尾に空白が含まれています");
            }

            if (extension.Contains(" "))
            {
                return (false, "拡張子に空白が含まれています");
            }

            return (true, "");
        }

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
