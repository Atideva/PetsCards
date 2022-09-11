#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace game.levels.stars
{
    [CustomEditor(typeof(StarsConfig))]
    public class StarsConfigEditor : Editor
    {
        StarsConfig _script;
        void OnEnable() => _script = (StarsConfig)target;

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();


            switch (_script.StarsRequirement)
            {
                case StarsRequirement.Points:
                    PointsAmountGUI(_script);
                    break;
                case StarsRequirement.TimeRemaining:
                    TimeRemainingGUI(_script);
                    break;
                default:
                    break;
            }

            if (GUI.changed) SetSceneDirty();

            GUI.backgroundColor = Color.red;

        }

        void PointsAmountGUI(StarsConfig script)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUIStyle style = new GUIStyle(EditorStyles.textField);
            style.normal.textColor = Color.yellow;

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Points required:",style);
            EditorGUILayout.Space();

            script.pointsRequiredForOneStar = EditorGUILayout.IntField("1 Stars (points)", script.pointsRequiredForOneStar);
            script.pointsRequiredForTwoStars = EditorGUILayout.IntField("2 Stars (points)", script.pointsRequiredForTwoStars);
            script.pointsRequiredForThreeStars = EditorGUILayout.IntField("3 Stars (points)", script.pointsRequiredForThreeStars);

            EditorGUILayout.EndVertical();
        }

        void TimeRemainingGUI(StarsConfig script)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUIStyle style = new GUIStyle(EditorStyles.textField);
            style.normal.textColor = Color.green;

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Seconds remaining:", style);
            EditorGUILayout.Space();

            script.timeRemainingForOneStar = EditorGUILayout.IntField("1 Stars (seconds)", script.timeRemainingForOneStar);
            script.timeRemainingForTwoStars = EditorGUILayout.IntField("2 Stars (seconds)", script.timeRemainingForTwoStars);
            script.timeRemainingForThreeStars = EditorGUILayout.IntField("3 Stars (seconds)", script.timeRemainingForThreeStars);

            EditorGUILayout.EndVertical();
        }


        void SetSceneDirty()
        {
            EditorUtility.SetDirty(_script);
            EditorUtility.SetDirty(_script.gameObject);
            EditorSceneManager.MarkSceneDirty(_script.gameObject.scene);
        }
    
    }
}

#endif
