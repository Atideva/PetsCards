using UnityEngine;

namespace systems.level_loader
{
    public class LoaderCallback : MonoBehaviour
    {
        bool _isFirstUpdate=true;

        void Update()
        {
            if (_isFirstUpdate)
            {
                _isFirstUpdate = false;
                Loader.LoaderCallback();
            }
        }
    }
}
