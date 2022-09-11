using UnityEngine;

namespace Imported.Effects.Particle_Attractor_by_Moonflower_Carnivore.Scripts
{
	public class Rotation : MonoBehaviour {
		public float xRotation = 0F;
		public float yRotation = 0F;
		public float zRotation = 0F;
		void OnEnable(){
			InvokeRepeating("Rotate", 0f, 0.0167f);
		}
		void OnDisable(){
			CancelInvoke();
		}
		public void clickOn(){
			InvokeRepeating("Rotate", 0f, 0.0167f);
		}
		public void ClickOff(){
			CancelInvoke();
		}
		void Rotate(){
			this.transform.localEulerAngles += new Vector3(xRotation,yRotation,zRotation);
		}
	}
}
