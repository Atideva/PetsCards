using UnityEngine;

namespace app.cash
{
   public class ResetSaves : MonoBehaviour
   {
      public void Reset()
      {
         PlayerPrefs.DeleteAll();
         Debug.LogWarning("Player prefs was CLEARED totally");
      }
   }
}
