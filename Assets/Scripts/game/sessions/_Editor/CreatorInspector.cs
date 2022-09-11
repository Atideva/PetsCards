#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace game.sessions._Editor
{
    [CustomEditor(typeof(Creator))]
    public class CreatorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            Creator creator = (Creator)target;

            var styleLabel = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            styleLabel.normal.textColor = Color.gray;
            var styleButton = new GUIStyle(GUI.skin.button); styleButton.normal.textColor = Color.green;

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Create action", styleLabel);

            EditorGUILayout.BeginHorizontal();


            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Timer", GUILayout.Height(20)))
            {
                creator.CreateTimerAction();
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Hitpoint", GUILayout.Height(20)))
            {
                creator.CreateHitpointAction();
            }


            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();



            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Create session", styleLabel);

            EditorGUILayout.Space();
            if (GUILayout.Button("Find Pair", GUILayout.Height(30)))
            {
                creator.CreateSession_FindPair();
            }

            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();

            DrawDefaultInspector();
        }
    }
}
#endif
