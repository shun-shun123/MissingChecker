using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MissingChecker
{
    public class MissingCheckerWindow : EditorWindow
    {
        private static MissingCheckerWindow _window;

        private readonly List<string> _searchPaths = new List<string>();

        [MenuItem("MissingChecker/Open window")]
        public static void Open()
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

        private void OnGUI()
        {
            DrawCheckPathList();

            if (_searchPaths.Count == 0)
            {
                return;
            }
            if (GUILayout.Button(Localization.Get("Execute")))
            {
            }
        }

        private void DrawCheckPathList()
        {
            using (new GUILayout.VerticalScope())
            {
                int i = 0;
                for (i = 0; i < _searchPaths.Count; i++)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        _searchPaths[i] = GUILayout.TextField(_searchPaths[i]);
                        if (GUILayout.Button(Localization.Get("X")))
                        {
                            break;
                        }
                    }
                }
                if (i != _searchPaths.Count)
                {
                    _searchPaths.RemoveAt(i);
                }

                if (GUILayout.Button(Localization.Get("+")))
                {
                    _searchPaths.Add("");
                }
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
