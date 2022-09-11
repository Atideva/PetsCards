//#if UNITY_EDITOR
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine;

//[CustomEditor(typeof(Card))]
//public class CardInspector : Editor
//{
//    Card script;
//    void OnEnable()
//    {
//        script = (Card)target;

//    }

//    public override void OnInspectorGUI()
//    {
 
//        DrawDefaultInspector();
//        if (GUI.changed) SetSceneDirty();

//    }
//    void SetSceneDirty()
//    {

//        EditorUtility.SetDirty(script);
//        EditorUtility.SetDirty(PrefabUtility.GetCorrespondingObjectFromSource(script.gameObject));
//        EditorUtility.SetDirty(script.gameObject);
//        EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
//    }
//}
//#endif