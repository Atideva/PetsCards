#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace fromWordSearch
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : Editor
    {
        public LevelConfig level;

        void OnEnable()
        {
            level = (LevelConfig)target;
        }

        public override void OnInspectorGUI()
        {

            //  var styleLabel = new GUIStyle { alignment = TextAnchor.MiddleCenter };
            //  styleLabel.normal.textColor = Color.gray;
            //  EditorGUILayout.LabelField("Create action", styleLabel);
            GUI.backgroundColor = Color.gray;
            GUI.contentColor = Color.gray;
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("helpbox");

            EditorGUILayout.ObjectField("Icon", level.Icon, typeof(Sprite), false);
            switch (level.Mode)
            {
                case LevelMode.Timer:
                    EditorGUILayout.ObjectField("Mode", Resources.Load("Timer"), typeof(Sprite), false);
                    break;
                case LevelMode.Lives:
                    EditorGUILayout.ObjectField("Mode", Resources.Load("Lives"), typeof(Sprite), false);
                    break;
                case LevelMode.None:
                    EditorGUILayout.ObjectField("Mode", Resources.Load("None"), typeof(Sprite), false);
                    break;
            }

            EditorGUILayout.EndVertical();
            //
            // EditorGUILayout.Space();
            // EditorGUILayout.Space();
            // EditorGUILayout.Space();
            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;
            base.OnInspectorGUI();
            if (GUI.changed) SetObjectDirty(level);
        }

        static void SetObjectDirty(Object obj)
        {
            EditorUtility.SetDirty(obj);
        }
    }
}
#endif