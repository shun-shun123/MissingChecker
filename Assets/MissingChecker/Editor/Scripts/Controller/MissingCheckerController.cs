using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MissingChecker
{
    public static class MissingCheckerController
    {
        /// <summary>
        /// Execute missing check process
        /// </summary>
        /// <param name="request">request data for executing missing checker</param>
        public static void ExecuteMissingCheck(ExecuteRequest request)
        {
            try
            {
                foreach (var path in request.Paths)
                {
                    ExecuteMissingCheckAtPath(path, request);
                }
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                request.OnException?.Invoke(ex);
                return;
            }
            request.OnSuccess?.Invoke();
        }

        /// <summary>
        /// Compare two records and extract diff of them.
        /// <remark>If there isn't comparable records at path, then skip.</remark>
        /// </summary>
        /// <param name="paths">search paths</param>
        public static void ExecuteDiffCheck(string[] paths)
        {
            foreach (var path in paths)
            {
                var latestReport = MissingCheckUtility.LoadReport(path, 0);
                var previousReport = MissingCheckUtility.LoadReport(path, 1);
                if (latestReport == null || previousReport == null)
                {
                    LogUtility.LogWarn($"Diff check failed because there aren't more than two records at {path}");
                    continue;
                }
                var diff = latestReport.Diff(previousReport);
                LogUtility.Log($"diff at {path}: {JsonUtility.ToJson(diff)}");
            }
        }

        private static void ExecuteMissingCheckAtPath(string path, ExecuteRequest request)
        {
            var assetPaths = AssetDatabase.GetAllAssetPaths()
                .Where(i =>
                {
                    return i.StartsWith(path);
                })
                .Where(i => {
                    var ext = Path.GetExtension(i);
                    return request.Extensions.Length == 0 || request.Extensions.Contains(ext);
                });
            foreach (var exporter in request.Exporters)
            {
                exporter.Reuse();
                exporter.SetTitle(path);
            }

            foreach (var assetPath in assetPaths)
            {
                LogUtility.Log($"Check at {assetPath}");
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                var hasMissing = MissingCheckUtility.HasMissingProperty(asset);
                request.OnChecked?.Invoke(assetPath, hasMissing);
                if (hasMissing)
                {
                    foreach (var exporter in request.Exporters)
                    {
                        exporter.Add(assetPath);
                    }
                }
            }

            foreach (var exporter in request.Exporters)
            {
                exporter.Export();
            }
        }
    }
}
