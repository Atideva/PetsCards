 
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{

   public void Restart()
   {
      Invoke(nameof(DoRestart),0.65f);
   }

   void DoRestart()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
   

}
