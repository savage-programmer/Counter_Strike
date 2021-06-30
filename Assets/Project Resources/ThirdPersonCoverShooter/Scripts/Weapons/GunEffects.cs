using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

namespace ThirdPersonCover
{
    /// <summary>
    /// Spawns effects and sounds on gun events.
    /// </summary>
    public class GunEffects : MonoBehaviour
    {
		
        /// <summary>
        /// Sound to play when ejecting a clip.
        /// </summary>
        [Tooltip("Sound to play when ejecting a clip.")]
        public AudioClip EjectSound;

        /// <summary>
        /// Sound to play when a clip is put inside the gun.
        /// </summary>
        [Tooltip("Sound to play when a clip is put inside the gun.")]
        public AudioClip RechamberSound;

        /// <summary>
        /// Possible sounds to play on each bullet fire.
        /// </summary>
        [Tooltip("Possible sounds to play on each bullet fire.")]
        public AudioClip[] FireSounds;

		public string poolNameForFireParticles;

        /// <summary>
        /// Object to instantiate when the gun is firing a bullet.
        /// </summary>
        [Tooltip("Object to instantiate when the gun is firing a bullet.")]
        public GameObject FireParticles;

		public Transform ShellPoint;

		public string poolNameForShellParticle;
        /// <summary>
        /// Object to instantiate when the gun is firing a bullet.
        /// </summary>
        [Tooltip("Object to instantiate when the gun is firing a bullet.")]
        public GameObject ShellParticle;

		private List<GameObject> _particles = new List<GameObject>();

		private Vector3 ShellPosition;

        /// <summary>
        /// Play a sequence of reload sounds.
        /// </summary>
        public void OnReloadStart()
        {
            StartCoroutine(reloadSequence());
        }

        /// <summary>
        /// Play fire effects delayed by the given amount of time in seconds.
        /// </summary>
        /// <param name="delay">Time to delay the creation of effects.</param>
        public void OnFire(float delay)
        {
            var gun = GetComponent<Gun>();

            if (gun != null && gun.Aim != null)
				StartCoroutine(playParticles(delay,"flashes", FireParticles, gun.Aim.transform, gun.Aim.transform.position, gun.Aim.transform.rotation));

			if (ShellPoint)
				ShellPosition = ShellPoint.position;
			else
				ShellPosition = transform.position;

			StartCoroutine(playParticles(delay,"shell", ShellParticle, null, ShellPosition, Quaternion.identity));
            StartCoroutine(playSound(delay, FireSounds));
        }

        private void LateUpdate()
        {
            int i = 0;
            while (i < _particles.Count)
            {
                if (_particles[i] == null)
                    _particles.RemoveAt(i);
                else
                    i++;
            }

            foreach (var particle in _particles)
                if (particle == null)
                    _particles.Remove(particle);
        }

        private void OnDisable()
        {
            foreach (var particle in _particles)
                if (particle != null)
                    GameObject.Destroy(particle);

            _particles.Clear();

            StopAllCoroutines();
        }

        private IEnumerator reloadSequence()
        {
            yield return new WaitForSeconds(0.1f);
            play(EjectSound);
            yield return new WaitForSeconds(0.6f);
            play(RechamberSound);
        }

        private void play(AudioClip clip)
        {
            if (clip != null)
                AudioSource.PlayClipAtPoint(clip, transform.position);
        }

        private IEnumerator playSound(float delay, AudioClip[] clips)
        {
            yield return new WaitForSeconds(delay);

            if (clips.Length > 0)
                play(clips[UnityEngine.Random.Range(0, clips.Length)]);
        }

		private IEnumerator playParticles(float delay,string poolName, GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, float destroyAfter = 3f)
        {
            if (prefab == null)
                yield break;

            yield return new WaitForSeconds(delay);


			var particle = PoolManager.Pools[poolName].Spawn(prefab); //GameObject.Instantiate(prefab);
		//	print (particle.gameObject.name);
//            particle.transform.parent = parent;
//            particle.transform.localPosition = position;
//            particle.transform.localRotation = rotation;

			particle.transform.position = position;
			particle.transform.rotation = rotation;
			particle.gameObject.SetActive(true);
//			_particles.Add(particle.gameObject);

//			if (particle.gameObject.GetComponent<Rigidbody> ()) 
//			{
//				particle.gameObject.GetComponent<Rigidbody> ().AddForce (((transform.right*75)+(transform.up*100)));
//				particle.gameObject.GetComponent<Rigidbody> ().AddTorque (particle.transform.up*5000);
//			}

//            GameObject.Destroy(particle, destroyAfter);
			PoolManager.Pools[poolName].Despawn(particle,destroyAfter);
        }
    }
}