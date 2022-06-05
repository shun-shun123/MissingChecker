using UnityEditor;

namespace MissingChecker
{
    public class Sandbox : EditorWindow
    {
        private static Sandbox _window;

        [MenuItem("Window/MissingChecker Sandbox")]
        public static void Open()
        {
            if (_window == null)
            {
                _window = GetWindow<Sandbox>();
            }
            _window.titleContent = new UnityEngine.GUIContent(L10n.Tr("Yes"));
            _window.Show();
        }

        [MenuItem("Window/MissingChecker/Save Execute Settings")]
        public static void SaveExecuteSetting()
        {
            var setting = new ExecuteSetting();
            setting.CheckAssetPaths = new System.Collections.Generic.List<string>(new string[]
            {
                "Assets/MissingChecker/",
                "Assets/Sample/",
                "Assets/Scenes/",
            });

            setting.CheckFileExtensions = new System.Collections.Generic.List<string>(new string[]
            {
                ".prefab",
                ".anim",
                ".mat"
            });
            LocalStorageUtility.SaveAsJson(setting, "setting_master.json");
        }

        [MenuItem("Window/MissingChecker/Load Execute Settings")]
        public static void LoadExecuteSetting()
        {
            var setting = LocalStorageUtility.Load<ExecuteSetting>("setting_master.json");
            if (setting.CheckAssetPaths == null)
            {
                UnityEngine.Debug.LogError("CheckAssetPaths is null");
                return;
            }
            if (setting.CheckFileExtensions == null)
            {
                UnityEngine.Debug.LogError("CheckFileExtensions is NULL");
                return;
            }
            foreach (var path in setting.CheckAssetPaths)
            {
                UnityEngine.Debug.Log($"path: {path}");
            }

            foreach (var ext in setting.CheckFileExtensions)
            {
                UnityEngine.Debug.Log($"ext: {ext}");
            }
        }

        [MenuItem("MissingChecker/Execute sample1")]
        public static void Execute01()
        {
            var setting = LocalStorageUtility.Load<ExecuteSetting>("setting_master.json");
            var request = new ExecuteRequest(
                setting.CheckAssetPaths, 
                setting.CheckFileExtensions, 
                exporters: new System.Collections.Generic.List<BaseExporter> { CsvExporter.Default, JsonExporter.Default }
            );
            MissingCheckerController.ExecuteMissingCheck(request);
        }

        [MenuItem("MissingChecker/Execute Diff Sample01")]
        public static void Execute02()
        {
            var setting = LocalStorageUtility.Load<ExecuteSetting>("setting_master.json");
            MissingCheckerController.ExecuteDiffCheck(setting.CheckAssetPaths.ToArray());
        }
    }
}
