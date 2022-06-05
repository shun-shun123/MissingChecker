using System.IO;
using UnityEngine;

namespace MissingChecker
{
    internal static class PathUtility 
    {
        internal static string Combine(string filePath)
        {
            return Path.Combine(Application.dataPath, $"../MissingChecker/{filePath}");
        }
    }
}
