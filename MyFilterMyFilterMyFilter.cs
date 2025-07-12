using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kotsuha
{
    public class MyFilterMyFilterMyFilter : EditorWindow
    {
        public const string WindowTitle = "My Filter";

        private List<(string condition, string stackTrace, LogType type)> _logs = new List<(string, string, LogType)>();
        private GUIStyle _boxStyle = null;
        private Vector2 _scrollPosition;

        [MenuItem("Window/Tools/" + WindowTitle)]
        public static MyFilterMyFilterMyFilter GetWindow()
        {
            return GetWindow<MyFilterMyFilterMyFilter>(WindowTitle);
        }

        void OnEnable()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        void OnGUI()
        {
            if (_boxStyle == null)
            {
                _boxStyle = GUI.skin.box;
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (var log in _logs)
            {
                EditorGUILayout.BeginVertical(_boxStyle);
                EditorGUILayout.TextField(log.condition);
                EditorGUILayout.TextArea(log.stackTrace);
                EditorGUILayout.EnumPopup(log.type);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Log \"Hello World!\""))
            {
                Debug.Log("Hello World!");
            }
        }

        void OnDisable()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            _logs.Add((condition, stackTrace, type));
        }
    }
}
