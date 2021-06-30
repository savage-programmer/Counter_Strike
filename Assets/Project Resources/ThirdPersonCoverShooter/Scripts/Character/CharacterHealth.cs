using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ThirdPersonCover
{
    /// <summary>
    /// Maintains character health.
    /// </summary>
    public class CharacterHealth : MonoBehaviour
    {
        /// <summary>
        /// Current health of the character.
        /// </summary>
        [Tooltip("Current health of the character.")]
		[Range(0, 4000)]
        public float Health = 300f;

        /// <summary>
        /// Max health to regenerate to.
        /// </summary>
        [Tooltip("Max health to regenerate to.")]
        [Range(0, 4000)]
        public float MaxHealth = 4000f;

        /// <summary>
        /// Amount of health regenerated per second.
        /// </summary>
        [Tooltip("Amount of health regenerated per second.")]
        public float Regeneration = 0f;

		public Image HealthImage;
		public GameObject bar; 

        private void LateUpdate()
        {
			if (bar) {
				bar.gameObject.transform.localScale = new Vector3 ((Health / MaxHealth), 1, 1);
			}

			if (HealthImage)
				HealthImage.fillAmount = Health / MaxHealth;
            Health = Mathf.Clamp(Health + Regeneration * Time.deltaTime, 0, MaxHealth);
        }

        /// <summary>
        /// Reduce health on bullet hit.
        /// </summary>
        public void OnHit(Hit hit)
		{
			Health -= hit.Damage;

			if (Health <= 0) {
				
				 if (this.gameObject.tag == "explosive"|| this.gameObject.tag == "MissionObj") {
					Debug.Log("explode");
					GameObject.FindWithTag ("control").GetComponent<RocketColisionChecker> ().show_damgaecar (this.gameObject);
				}
				else
					SendMessage ("OnDead");
			}
		}
    }
}