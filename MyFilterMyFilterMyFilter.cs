using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kotsuha
{
    public class MyFilterMyFilterMyFilter : EditorWindow, IHasCustomMenu
    {
        public const string WindowTitle = "My Filter";

        private List<LogData> _logs = new List<LogData>();
        private MyGuiContents _guiContents;
        private MyGuiStyles _guiStyles;
        private MyGuiOptions _guiOptions;
        private Vector2 _scrollPosition;

        private class LogData
        {
            public string Condition;
            public string StackTrace;
            public LogType Type;
            public bool Expanded;

            public LogData(string condition, string stackTrace, LogType type, bool expanded)
            {
                Condition = condition;
                StackTrace = stackTrace;
                Type = type;
                Expanded = expanded;
            }
        }

        private class MyGuiContents
        {
            public GUIContent TitleContent;

            public MyGuiContents()
            {
                TitleContent = new GUIContent();
                TitleContent.text = WindowTitle;
                TitleContent.image = EditorGUIUtility.IconContent("UnityEditor.ConsoleWindow").image;
            }
        }

        private class MyGuiStyles
        {
            public GUIStyle BoxStyle;
            public GUIStyle LogStyle1;
            public GUIStyle WarningStyle1;
            public GUIStyle LogStyle2;
            public GUIStyle WarningStyle2;
            public GUIStyle ErrorStyle1;
            public GUIStyle ErrorStyle2;

            public MyGuiStyles()
            {
                BoxStyle = GUI.skin.box;

                LogStyle1 = new GUIStyle(EditorStyles.label);
                LogStyle1.normal.textColor = Color.white;

                LogStyle2 = new GUIStyle(EditorStyles.wordWrappedLabel);
                LogStyle2.normal.textColor = Color.white;

                WarningStyle1 = new GUIStyle(EditorStyles.label);
                WarningStyle1.normal.textColor = Color.yellow;

                WarningStyle2 = new GUIStyle(EditorStyles.wordWrappedLabel);
                WarningStyle2.normal.textColor = Color.yellow;

                ErrorStyle1 = new GUIStyle(EditorStyles.label);
                ErrorStyle1.normal.textColor = Color.red;

                ErrorStyle2 = new GUIStyle(EditorStyles.wordWrappedLabel);
                ErrorStyle2.normal.textColor = Color.red;
            }
        }

        private class MyGuiOptions
        {
            public GUILayoutOption DontExpandWidth = GUILayout.ExpandWidth(false);
        }

        [MenuItem("Window/Tools/" + WindowTitle)]
        public static MyFilterMyFilterMyFilter GetWindow()
        {
            return GetWindow<MyFilterMyFilterMyFilter>();
        }

        void Awake()
        {
            _guiContents ??= new MyGuiContents();

            titleContent = _guiContents.TitleContent;
        }

        void OnEnable()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
        }

        void OnGUI()
        {
            _guiStyles ??= new MyGuiStyles();
            _guiOptions ??= new MyGuiOptions();

            DrawToolbar();
            // GUILayout.BeginHorizontal(EditorStyles.toolbar);
            // if (GUILayout.Toggle(true, "Logs", EditorStyles.toolbarButton)) { }
            // if (GUILayout.Toggle(true, "Warnings", EditorStyles.toolbarButton)) { }
            // if (GUILayout.Toggle(true, "Errors", EditorStyles.toolbarButton)) { }
            // GUILayout.EndHorizontal();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            // foreach (var log in _logs)
            // {
            //     EditorGUILayout.BeginVertical(_myStyles.BoxStyle);
            //     EditorGUILayout.TextField(log.condition);
            //     EditorGUILayout.TextArea(log.stackTrace);
            //     EditorGUILayout.EnumPopup(log.type);
            //     EditorGUILayout.EndVertical();
            // }
            foreach (var log in _logs)
            {
                EditorGUILayout.BeginVertical(_guiStyles.BoxStyle);

                GUIStyle logStyle1;
                GUIStyle logStyle2;
                switch (log.Type)
                {
                    case LogType.Error:
                    case LogType.Assert:
                    case LogType.Exception:
                        logStyle1 = _guiStyles.ErrorStyle1;
                        logStyle2 = _guiStyles.ErrorStyle2;
                        break;
                    case LogType.Warning:
                        logStyle1 = _guiStyles.WarningStyle1;
                        logStyle2 = _guiStyles.WarningStyle2;
                        break;
                    case LogType.Log:
                    default:
                        logStyle1 = _guiStyles.LogStyle1;
                        logStyle2 = _guiStyles.LogStyle2;
                        break;
                }
                log.Expanded = EditorGUILayout.Foldout(log.Expanded, log.Condition, true, logStyle1);
                if (log.Expanded)
                {
                    EditorGUILayout.LabelField(log.StackTrace, logStyle2);
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton, _guiOptions.DontExpandWidth))
            {
                Clear();
            }
            if (GUILayout.Button("LOG", EditorStyles.toolbarButton, _guiOptions.DontExpandWidth))
            {
                Debug.Log("Hello World!");
            }
            if (GUILayout.Button("WAR", EditorStyles.toolbarButton, _guiOptions.DontExpandWidth))
            {
                Debug.LogWarning("Hello World!");
            }
            if (GUILayout.Button("ERR", EditorStyles.toolbarButton, _guiOptions.DontExpandWidth))
            {
                Debug.LogError("Hello World!");
            }
            EditorGUILayout.EndHorizontal();
        }

        void OnDisable()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            _logs.Add(new LogData(condition, stackTrace, type, false));
        }

        public void Clear()
        {
            _logs.Clear();
        }
    }
}
