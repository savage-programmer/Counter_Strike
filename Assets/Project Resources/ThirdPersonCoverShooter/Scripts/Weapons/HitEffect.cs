﻿using UnityEngine;
using PathologicalGames;

namespace ThirdPersonCover
{
    /// <summary>
    /// Plays an effect (instantiates a prefab) everytime a Gun hit's an object with this script.
    /// </summary>
    public class HitEffect : MonoBehaviour
    {
        /// <summary>
        /// Effect to be instantiated on the point of bullet impact.
        /// </summary>
        [Tooltip("Effect to be instantiated on the point of bullet impact.")]
        public GameObject Effect;

        /// <summary>
        /// Time to wait before destroying an instantiated effect object.
        /// </summary>
        [Tooltip("Time to wait before destroying an instantiated effect object.")]
        public float DestroyAfter = 5;

        /// <summary>
        /// Spawns the particle effect on hit position.
        /// </summary>
        public void OnHit(Hit hit)
        {
            if (Effect == null)
                return;

//			var effect = GameObject.Instantiate(Effect);
			var effect = PoolManager.Pools["hits"].Spawn(Effect);

//            effect.transform.parent = null;
			effect.transform.position = hit.Position;//+ hit.Normal * 0.1f;
			effect.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.Normal);
			effect.gameObject.SetActive(true);
//			GameObject.Destroy(effect, 4);
			PoolManager.Pools["hits"].Despawn(effect, 4);

        }

        private void OnValidate()
        {
            DestroyAfter = Mathf.Max(0, DestroyAfter);
        }
    }
}