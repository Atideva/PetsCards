#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace app.cash
{
    [CustomEditor(typeof(ResetSaves))]
    public class ResetSavesEditor : Editor
    {
    
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            ResetSaves creator = (ResetSaves)target;

            var styleLabel = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            styleLabel.normal.textColor = Color.gray;

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
        
            EditorGUILayout.LabelField("Clear all application cash", styleLabel);
            EditorGUILayout.Space();

            if (GUILayout.Button("Reset saves", GUILayout.Height(30)))
            {
                creator.Reset();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();


        }
    
    
    }
}

#endif
