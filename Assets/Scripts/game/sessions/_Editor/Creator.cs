#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace game.sessions._Editor
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(CreatorConfig))]
    public class Creator : MonoBehaviour
    {
        CreatorConfig _setup;
        void Start() => _setup = GetComponent<CreatorConfig>();

        void LateUpdate()
        {
            if (!_setup) _setup = GetComponent<CreatorConfig>();
        }


        public void CreateTimerAction() => CreateObject(_setup.creatorData.timerAction);
        public void CreateHitpointAction() => CreateObject(_setup.creatorData.hitpointsAction);
        public void CreateSession_FindPair() => CreateObject(_setup.creatorData.findPairs);


        void CreateObject(GameObject prefab)
        {
            var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            obj.transform.parent = transform;
        }
    }
}

#endif