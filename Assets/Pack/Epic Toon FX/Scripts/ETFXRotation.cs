using UnityEngine;

namespace Imported.Epic_Toon_FX.Scripts
{
    public class EtfxRotation : MonoBehaviour
    {
 
        [Header("Rotate axises by degrees per second")]
        public Vector3 rotateVector = Vector3.zero;
 
        public enum SpaceEnum { Local, World };
        public SpaceEnum rotateSpace;
 
        // Use this for initialization
        void Start()
        {
 
        }
 
        // Update is called once per frame
        void Update()
        {
            if (rotateSpace == SpaceEnum.Local)
                transform.Rotate(rotateVector * Time.deltaTime);
            if (rotateSpace == SpaceEnum.World)
                transform.Rotate(rotateVector * Time.deltaTime, Space.World);
        }
    }
}