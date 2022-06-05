using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace MissingChecker
{
    internal static class MissingCheckUtility
    {
        internal static Report LoadReport(string path, int fromLatest)
        {
            string[] reportFiles;
            try
            {
                reportFiles = Directory.GetFiles(path)
                    .Where(i => Path.GetExtension(i) == ".json")
                    .OrderBy(i =>
                    {
                        var fileInfo = new FileInfo(i);
                        return fileInfo.LastWriteTime;
                    })
                    .ToArray();
                if (reportFiles.Length <= fromLatest)
                {
                    return null;
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                LogUtility.LogException(ex);
                return null;
            }
            catch (NullReferenceException ex)
            {
                LogUtility.LogException(ex);
                return null;
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }

            var report = LocalStorageUtility.Load<Report>(reportFiles[fromLatest]);
            return report;
        }

        internal static bool HasMissingProperty(UnityEngine.Object target)
        {
            if (target == null)
            {
                return false;
            }
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.GetIterator();

            if (HasMissingProperty(property))
            {
                return true;
            }

            if (target is UnityEngine.GameObject gameObject && HasMissingProperty(gameObject))
            {
                return true;
            }
            return false;
        }

        internal static UnityEngine.Object[] GetAllAssetsAtPath(string path)
        {
            // prevent error "Do not use ReadObjectThreaded on scene objects"
            if (typeof(SceneAsset) == AssetDatabase.GetMainAssetTypeAtPath(path))
            {
                LogUtility.LogWarn($"{path} is excluded from searching because of preventing error \"Do not use ReadObjectThreaded on scene objects\"");
                return Array.Empty<UnityEngine.Object>();
            }
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            return assets;
        }

        private static bool HasMissingProperty(UnityEngine.GameObject target)
        {
            if (target == null)
            {
                throw new ArgumentNullException();
            }
            var components = target.GetComponents<UnityEngine.Component>();
            if (HasMissingProperty(components))
            {
                return true;
            }
            return false;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasMissingProperty(UnityEngine.Component[] components)
        {
            foreach (var c in components)
            {
                if (c == null)
                {
                    continue;
                }
                var sObject = new SerializedObject(c);
                var sProps = sObject.GetIterator();
                if (HasMissingProperty(sProps))
                {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasMissingProperty(SerializedProperty property)
        {
            while (property.Next(true))
            {
                if (IsMissing(property))
                {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsMissing(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference &&
                property.objectReferenceValue == null &&
                property.objectReferenceInstanceIDValue != 0;
        }
    }
}
