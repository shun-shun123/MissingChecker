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
                return (false, "�p�X���̖����ɋ󔒂��܂܂�Ă��܂�");
            }

            if (!path.StartsWith("Assets/"))
            {
                return (false, "�`�F�b�N�Ώۂ�Assets/�ȉ��ł���K�v������܂�");
            }

            if (!path.EndsWith("/"))
            {
                return (false, "�`�F�b�N�Ώۂ̃p�X����\"/\"�ŏI���K�v������܂�"); 
            }

            var invalidPathChars = Path.GetInvalidPathChars();
            foreach (var c in invalidPathChars)
            {
                if (path.Contains(c.ToString()))
                {
                    return (false, $"�p�X���ł͎g���Ȃ�{c}���܂܂�Ă��܂�");
                }
            }
            return (true, "");
        }

        internal static (bool, string) ValidateCheckExtension(string extension)
        {
            if (!extension.StartsWith("."))
            {
                return (false, "�g���q��\".\"����n�܂�K�v������܂�");
            }

            if (extension.EndsWith(" "))
            {
                return (false, "�g���q�̖����ɋ󔒂��܂܂�Ă��܂�");
            }

            if (extension.Contains(" "))
            {
                return (false, "�g���q�ɋ󔒂��܂܂�Ă��܂�");
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
