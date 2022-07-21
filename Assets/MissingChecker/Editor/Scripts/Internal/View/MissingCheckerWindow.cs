using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MissingChecker
{
    internal class MissingCheckerWindow : EditorWindow
    {
        private const int DEFAULT_UI_SIZE = 24;

        private const float DEFAULT_SPACE = 8f;

        private static MissingCheckerWindow _window;

        private ExecuteSetting _executeSetting = new ExecuteSetting();

        private GUIContent _deleteButtonTexture;
        private GUIContent _createNewButtonTexture;

        [MenuItem("MissingChecker/Open window")]
        internal static void Open()
        {
            if (_window == null)
            {
                _window = GetWindow<MissingCheckerWindow>();
            }
            _window.OnShowWindow();
            _window.Show();
        }

        private void OnShowWindow()
        {
            titleContent = new UnityEngine.GUIContent(Localization.Get("MissingChecker"));
        }

        private void OnEnable()
        {
            _deleteButtonTexture = EditorGUIUtility.IconContent("winbtn_win_close");
            _createNewButtonTexture = EditorGUIUtility.IconContent("CreateAddNew");
        }

        private void OnGUI()
        {
            DrawCheckPathList();
            DrawCheckExtensionList();

            if (_executeSetting.CheckAssetPaths.Count == 0)
            {
                return;
            }
            DrawReadWritePreset();

            if (GUILayout.Button(Localization.Get("Execute")))
            {
                var request = new ExecuteRequest(_executeSetting);
                MissingCheckerController.ExecuteMissingCheck(request);
            }
        }

        private void DrawCheckPathList()
        {
            GUILayout.Label("Check asset path", EditorStyles.boldLabel);
            using (new GUILayout.VerticalScope())
            {
                int i = 0;
                for (i = 0; i < _executeSetting.CheckAssetPaths.Count; i++)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(_deleteButtonTexture, GUILayout.Width(DEFAULT_UI_SIZE), GUILayout.Height(DEFAULT_UI_SIZE)))
                        {
                            break;
                        }
                        _executeSetting.CheckAssetPaths[i] = GUILayout.TextField(_executeSetting.CheckAssetPaths[i], GUILayout.Height(DEFAULT_UI_SIZE));
                    }
                    (var valid, string cause) = PathUtility.ValidateCheckPath(_executeSetting.CheckAssetPaths[i]);
                    if (!valid)
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            var warnIcon = EditorGUIUtility.IconContent("Warning");
                            GUILayout.Box(warnIcon, GUIStyle.none);
                            GUILayout.Label(cause, EditorStyles.boldLabel);
                            GUILayout.FlexibleSpace();
                        }
                    }
                    GUILayout.Space(DEFAULT_SPACE);
                }
                if (i != _executeSetting.CheckAssetPaths.Count)
                {
                    _executeSetting.CheckAssetPaths.RemoveAt(i);
                }

                if (GUILayout.Button(_createNewButtonTexture))
                {
                    _executeSetting.CheckAssetPaths.Add("");
                }
            }
        }

        private void DrawCheckExtensionList()
        {
            GUILayout.Label("Check file extensions", EditorStyles.boldLabel);
            using (new GUILayout.VerticalScope())
            {
                int i = 0;
                for (i = 0; i < _executeSetting.CheckFileExtensions.Count; i++)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(_deleteButtonTexture, GUILayout.Width(DEFAULT_UI_SIZE), GUILayout.Height(DEFAULT_UI_SIZE)))
                        {
                            break;
                        }
                        _executeSetting.CheckFileExtensions[i] = GUILayout.TextField(_executeSetting.CheckFileExtensions[i], GUILayout.Height(DEFAULT_UI_SIZE));
                    }
                    (var valid, string cause) = PathUtility.ValidateCheckExtension(_executeSetting.CheckFileExtensions[i]);
                    if (!valid)
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            var warnIcon = EditorGUIUtility.IconContent("Warning");
                            GUILayout.Box(warnIcon, GUIStyle.none, GUILayout.Width(DEFAULT_UI_SIZE), GUILayout.Height(DEFAULT_UI_SIZE));
                            GUILayout.Label(cause, EditorStyles.boldLabel);
                            GUILayout.FlexibleSpace();
                        }
                    }
                    GUILayout.Space(DEFAULT_SPACE);
                }
                if (i != _executeSetting.CheckFileExtensions.Count)
                {
                    _executeSetting.CheckFileExtensions.RemoveAt(i);
                }

                if (GUILayout.Button(_createNewButtonTexture))
                {
                    _executeSetting.CheckFileExtensions.Add("");
                }
            }
        }

        private void DrawReadWritePreset()
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button(Localization.Get("Save Preset")))
                {
                    ShowSavePresetConfirmDialog();
                }
                if (GUILayout.Button(Localization.Get("Load Preset")))
                {
                    ShowLoadPresetConfirmDialog();
                }
            }
        }

        private void ShowSavePresetConfirmDialog()
        {
            if (!Directory.Exists(PathUtility.PresetDirectory))
            {
                Directory.CreateDirectory(PathUtility.PresetDirectory);
            }
            var savedPath = EditorUtility.SaveFilePanel("Save preset", PathUtility.PresetDirectory, "new_preset", "json");
            if (string.IsNullOrEmpty(savedPath))
            {
                return;
            }

            var savedDirectory = PathUtility.GetDirectoryName(savedPath);
            var savedDirectoryInfo = PathUtility.GetDirectoryInfo(savedDirectory);
            var targetDirectoryInfo = PathUtility.GetDirectoryInfo(PathUtility.PresetDirectory);
            if (savedDirectoryInfo == targetDirectoryInfo)
            {
                EditorUtility.DisplayDialog(Localization.Get("Warning"), $"Preset can't be saved at {savedDirectory}\nPreset must be saved at {PathUtility.PresetDirectory}", "Close");
                return;
            }

            var savedFileName = PathUtility.GetFileName(savedPath);
            LocalStorageUtility.SaveAsJson(_executeSetting, $"{PathUtility.PRESET}/{savedFileName}.json");
        }

        private void ShowLoadPresetConfirmDialog()
        {
            var loadPath = EditorUtility.OpenFilePanel("Load Preset", PathUtility.PresetDirectory, "json");
            if (string.IsNullOrEmpty(loadPath))
            {
                return;
            }
            var preset = LocalStorageUtility.LoadFromAbsolutePath<ExecuteSetting>(loadPath);
            if (preset != null)
            {
                _executeSetting = preset;
            }
        }

        private static void OnSearchAt(string path)
        {
            EditorUtility.DisplayProgressBar("Checking assets", $"Checking at {path}", 0.0f);
        }

        private static void OnSuccess()
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("MissingChecker", "Complete checking", "OK");
        }

        private static void OnException(Exception ex)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("MissingChecker Failed", $"{ex}\n{ex.Message}", "Close");
        }
    }
}
