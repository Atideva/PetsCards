using game.managers;
using UnityEngine;

namespace game.sessions.type
{
    [ExecuteInEditMode]
    public class HitpointMode : MonoBehaviour, ISession
    {

        #region EDITOR RENAME
#if UNITY_EDITOR
        string _defaultName;
        void Start() => _defaultName = "Hitpoints";
        void LateUpdate()
        {
            var newName = _defaultName + ": " + NameSuffix;
            if (newName != gameObject.name) gameObject.name = newName;
        }
        string NameSuffix => hitpointsAction.ToString();
#endif
        #endregion

  
        public HitpointsActions hitpointsAction;

        public enum HitpointsActions
        {
            Enable,
            Disable
        }


        public void StartSession()
        {
            DoAction();
            Events.Instance.SessionComplete();
        }
        public void Request()
        {

        }



        void DoAction()
        {
            switch (hitpointsAction)
            {
                case HitpointsActions.Enable:
                    Events.Instance.HitpointsEnable();
                    break;
                case HitpointsActions.Disable:
                    Events.Instance.HitpointsDisable();
                    break;
            }
        }
 
 

    }
}
