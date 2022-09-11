using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace Analytics
{
    public class FirebaseInit : MonoBehaviour
    {
        void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        }
    }
}