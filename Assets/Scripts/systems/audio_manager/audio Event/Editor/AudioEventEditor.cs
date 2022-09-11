using UnityEngine;
using System.Collections;
using systems.audio_manager.audio_Event;
using UnityEditor;

[CustomEditor(typeof(SoundData), true)]
public class AudioEventEditor : Editor
{

    [SerializeField] private AudioSource _previewer;

    public void OnEnable()
    {
        _previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        DestroyImmediate(_previewer.gameObject);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

        if (GUILayout.Button("Preview"))
        {
            ((SoundData)target).EditorTest(_previewer);
        }
        EditorGUI.EndDisabledGroup();
    }
}
