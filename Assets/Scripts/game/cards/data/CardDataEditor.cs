#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace game.cards.data
{
    [CustomEditor(typeof(CardData))]
    public class CardDataEditor : Editor
    {
        public CardData card;

        void OnEnable()
        {
            card = (CardData) target;
        }

        public override void OnInspectorGUI()
        {
            //  var styleLabel = new GUIStyle { alignment = TextAnchor.MiddleCenter };
            //  styleLabel.normal.textColor = Color.gray;
            //  EditorGUILayout.LabelField("Create action", styleLabel);
            //      GUI.backgroundColor = Color.gray;
            //    GUI.contentColor = Color.gray;
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUILayout.ObjectField("Icon", card.CardPrefab.MainArt.sprite, typeof(Sprite), false);
            EditorGUILayout.EndVertical();
 
            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;
            base.OnInspectorGUI();
            if (GUI.changed) SetObjectDirty(card);
        }

        static void SetObjectDirty(Object obj)
        {
            EditorUtility.SetDirty(obj);
        }
    }
}
#endif