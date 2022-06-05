using System;
using System.IO;
using UnityEngine;

namespace MissingChecker
{
    internal static class MissingCheckerCLI
    {
        internal static void ExecuteMissingCheck()
        {
            var presetName = GetPresetFileName();
            var setting = LoadExecuteSettingPreset(presetName);
            var executeRequest = new ExecuteRequest(setting);
            MissingCheckerController.ExecuteMissingCheck(executeRequest);
        }

        internal static void ExecuteDiffCheck()
        {
            var presetName = GetPresetFileName();
            var setting = LoadExecuteSettingPreset(presetName);
            MissingCheckerController.ExecuteDiffCheck(setting.CheckAssetPaths.ToArray());
        }

        private static ExecuteSetting LoadExecuteSettingPreset(string presetName)
        {
            var presetFullPath = PathUtility.PresetDirectory + $"/{presetName}";
            var setting = LocalStorageUtility.LoadFromAbsolutePath<ExecuteSetting>(presetFullPath);
            if (setting == null)
            {
                throw new FileNotFoundException($"{presetFullPath} is not found");
            }
            return setting;
        }

        private static string GetPresetFileName()
        {
            var args = Environment.GetCommandLineArgs();
            var index = Array.IndexOf(args, "-preset");
            if (index == -1)
            {
                throw new ArgumentNullException("-preset argument is not found");
            }
            return args[index + 1];
        }
    }
}
