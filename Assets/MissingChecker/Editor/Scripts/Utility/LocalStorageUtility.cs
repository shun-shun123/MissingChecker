using System;
using System.IO;
using UnityEngine;

namespace MissingChecker
{
    internal static class LocalStorageUtility
    {
        private static string _savedDirectory => Path.Combine(Application.dataPath, "../MissingChecker");

        internal static string TimeAsString => DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");

        internal static T Load<T>(string fileName)
        {
            T data = default;
            var filePath = Path.Combine(_savedDirectory, fileName);
            if (!File.Exists(filePath))
            {
                return data;
            }
            using (var sr = new StreamReader(File.Open(filePath, FileMode.Open)))
            {
                var content = sr.ReadToEnd();
                data = JsonUtility.FromJson<T>(content);
            }
            return data;
        }

        internal static void SaveAsJson<T>(T data, string fileName)
        {
            try
            {
                var json = JsonUtility.ToJson(data, true);
                Save(json, fileName);
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        internal static void Save(string content, string fileName)
        {
            try
            {
                var filePath = Path.Combine(_savedDirectory, fileName);
                var directory = Path.GetDirectoryName(filePath);
                CreateSaveDirectoryIfNeed(directory);
                using (var sw = new StreamWriter(File.Open(filePath, FileMode.OpenOrCreate)))
                {
                    sw.Write(content);
                }
                LogUtility.Log($"saved at {filePath}\ncontent: {content}");
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        private static void CreateSaveDirectoryIfNeed(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}