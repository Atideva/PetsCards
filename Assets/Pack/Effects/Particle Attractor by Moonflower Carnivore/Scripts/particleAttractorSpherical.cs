using UnityEngine;

namespace Imported.Effects.Particle_Attractor_by_Moonflower_Carnivore.Scripts
{
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleAttractorSpherical : MonoBehaviour {
		ParticleSystem _ps;
		ParticleSystem.Particle[] _mParticles;
		public Transform target;
		public float speed = 5f;
		int _numParticlesAlive;
		void Start () {
			_ps = GetComponent<ParticleSystem>();
			if (!GetComponent<Transform>()){
				GetComponent<Transform>();
			}
		}
		void Update () {
			_mParticles = new ParticleSystem.Particle[_ps.main.maxParticles];
			_numParticlesAlive = _ps.GetParticles(_mParticles);
			float step = speed * Time.deltaTime;
			for (int i = 0; i < _numParticlesAlive; i++) {
				_mParticles[i].position = Vector3.SlerpUnclamped(_mParticles[i].position, target.position, step);
			}
			_ps.SetParticles(_mParticles, _numParticlesAlive);
		}
	}
}
