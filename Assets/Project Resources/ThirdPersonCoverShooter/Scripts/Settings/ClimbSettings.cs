using System;
using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Climbing settings for a character.
    /// </summary>
    [Serializable]
    public struct ClimbSettings
    {
        /// <summary>
        /// Capsule height to set during a climb.
        /// </summary>
        [Tooltip("Capsule height to set during a climb.")]
        [Range(0, 10)]
        public float CapsuleHeight;

        /// <summary>
        /// Moment in climbing animation to turn the character gravity on.
        /// </summary>
        [Tooltip("Moment in climbing animation to turn the character gravity on.")]
        [Range(0, 1)]
        public float FallTime;

        /// <summary>
        /// Default character climbing settings.
        /// </summary>

        public static ClimbSettings Default()
        {
            var settings = new ClimbSettings();
            settings.CapsuleHeight = 1.5f;
            settings.FallTime = 0.55f;

            return settings;
        }
    }
}