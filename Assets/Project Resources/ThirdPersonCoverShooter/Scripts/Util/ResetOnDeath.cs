using System.Collections;
using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Reset the level on character's death.
    /// </summary>
    public class ResetOnDeath : MonoBehaviour
    {
        /// <summary>
        /// Time in seconds to reset the level after character's death
        /// </summary>
        [Tooltip("Time in seconds to reset the level after character's death")]
        public float Delay = 3.0f;

        /// <summary>
        /// Starts a sequence to reset the level after waiting for Delay.
        /// </summary>
        public void OnDead()
        {
            StartCoroutine(delayedReset());
        }

        private IEnumerator delayedReset()
        {
            yield return new WaitForSeconds(Delay);
            Application.LoadLevel(Application.loadedLevel);
        }

        private void OnValidate()
        {
            Delay = Mathf.Max(0, Delay);
        }
    }
}