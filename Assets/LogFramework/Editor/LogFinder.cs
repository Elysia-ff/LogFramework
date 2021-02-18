using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace LogFramework
{
    public class LogFinder : EditorWindow
    {
        public class FlagData
        {
            public string name;
            public bool value;

            public FlagData(string _name)
            {
                name = _name;
                value = true;
            }
        }

        public class ScrollData
        {
            public readonly int logNameWidth = 300;
            public readonly int logFlagWidth = 130;
            public readonly int lineHeight = 20;

            public Rect rect;
            public Vector3 scrollPos;
            public int startIdx;
            public int endIdx;
            public List<LogContainer.Data> logs = new List<LogContainer.Data>();
        }

        private readonly Dictionary<LogFlagType, FlagData> flagFilter = new Dictionary<LogFlagType, FlagData>()
        {
            { LogFlagType.General, new FlagData("General Logs") },
            { LogFlagType.Special, new FlagData("Special Logs") }
        };

        private string filterText = string.Empty;
        private ScrollData scrollData = new ScrollData();

        private LogContainer logContainer = new LogContainer();

        [MenuItem("Window/Log Finder")]
        public static void Init()
        {
            // TODO (LogFramework) ::
            // This is to load sample table.
            // Replace with yours or delete.
            Sample.SampleTable t = Sample.SampleTable.Table;

            LogFinder window = GetWindow<LogFinder>("Log Finder");
            window.Initialize();
        }

        public void Initialize()
        {
            logContainer.MakeLogList();
            scrollData.logs = logContainer.LogList;
        }

        private void OnGUI()
        {
            using (EditorGUI.ChangeCheckScope changeScope = new EditorGUI.ChangeCheckScope())
            {
                GUILayout.Label("Flag Filter", EditorStyles.boldLabel);
                foreach (KeyValuePair<LogFlagType, FlagData> kv in flagFilter)
                {
                    flagFilter[kv.Key].value = EditorGUILayout.Toggle(kv.Value.name, kv.Value.value);
                }
                GUILayout.Space(10);

                GUILayout.Label("Log Filter", EditorStyles.boldLabel);
                filterText = EditorGUILayout.TextField(filterText);
                GUILayout.Space(10);

                if (changeScope.changed)
                {
                    scrollData.logs = SearchLog();
                    scrollData.scrollPos = Vector2.zero;
                }
            }

            GUILayout.Label(string.Format("Found {0} log(s)", scrollData.logs.Count));
            using (new EditorGUILayout.HorizontalScope())
            {
                Color prevColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.grey;
                GUILayout.Toolbar(-1, new string[] { "LogName" }, EditorStyles.toolbarButton, GUILayout.Width(scrollData.logNameWidth));
                GUILayout.Toolbar(-1, new string[] { "Flag" }, EditorStyles.toolbarButton, GUILayout.Width(scrollData.logFlagWidth));
                GUILayout.Toolbar(-1, new string[] { "Description" }, EditorStyles.toolbarButton, GUILayout.ExpandWidth(true));
                GUI.backgroundColor = prevColor;
            }

            using (EditorGUILayout.ScrollViewScope scroll = new EditorGUILayout.ScrollViewScope(scrollData.scrollPos))
            {
                scrollData.scrollPos = scroll.scrollPosition;
                if (Event.current.type == EventType.Layout)
                {
                    CalculateScrollIndex(scrollData.logs, ref scrollData.startIdx, ref scrollData.endIdx);
                }

                for (int i = 0; i < scrollData.logs.Count; ++i)
                {
                    if (i >= scrollData.startIdx && i <= scrollData.endIdx)
                    {
                        using (new EditorGUILayout.HorizontalScope("box", GUILayout.Height(scrollData.lineHeight)))
                        {
                            GUILayout.Label(scrollData.logs[i].Log.LogName, GUILayout.Width(scrollData.logNameWidth));
                            GUILayout.Label(flagFilter[scrollData.logs[i].Log.LogFlag].name, GUILayout.Width(scrollData.logFlagWidth));
                            GUILayout.Label(scrollData.logs[i].Desc, GUILayout.ExpandWidth(true));
                        }
                    }
                    else
                    {
                        GUILayout.Space(scrollData.lineHeight);
                    }
                }
            }

            if (Event.current.type == EventType.Repaint)
            {
                Rect newRect = GUILayoutUtility.GetLastRect();
                if (scrollData.rect != newRect)
                {
                    scrollData.rect = newRect;
                    Repaint();
                }
            }
        }

        private List<LogContainer.Data> SearchLog()
        {
            List<LogContainer.Data> logList = logContainer.LogList;
            List<LogContainer.Data> filteredLogs = new List<LogContainer.Data>();
            for (int i = 0; i < logList.Count; ++i)
            {
                if (flagFilter[logList[i].Log.LogFlag].value)
                {
                    if (string.IsNullOrEmpty(filterText) || logList[i].Log.LogName.ToLower().Contains(filterText.ToLower()))
                    {
                        filteredLogs.Add(logList[i]);
                    }
                }
            }

            return filteredLogs;
        }

        private void CalculateScrollIndex(List<LogContainer.Data> datas, ref int start, ref int end)
        {
            start = 0;
            end = -1;
            float totalHeight = 0;
            for (int i = 0; i < datas.Count; ++i)
            {
                float h = scrollData.lineHeight;
                if (scrollData.scrollPos.y <= totalHeight + h)
                {
                    start = i;
                    break;
                }

                totalHeight += h;
            }

            for (int i = start; i < datas.Count; ++i)
            {
                float h = scrollData.lineHeight;
                if (scrollData.scrollPos.y + scrollData.rect.height >= totalHeight)
                {
                    end = i;
                }
                else
                {
                    break;
                }

                totalHeight += h;
            }
        }
    }
}
