using UnityEngine;

namespace game.sessions._Editor
{
    [CreateAssetMenu(fileName = "Session creator", menuName = "Data/Session creator data")]
    public class CreatorData : ScriptableObject
    {
        [Header("Session elements")]
        public GameObject timerAction;
        public GameObject hitpointsAction;

        [Header("Sessions prefabs")]
        public GameObject findPairs;


    }
}
