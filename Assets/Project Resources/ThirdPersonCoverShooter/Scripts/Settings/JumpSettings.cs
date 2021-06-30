﻿using System;
using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Character jump settings.
    /// </summary>
    [Serializable]
    public struct JumpSettings
    {
        /// <summary>
        /// Jump up velocity.
        /// </summary>
        [Tooltip("Jump up velocity.")]
        [Range(0, 20)]
        public float Strength;

        /// <summary>
        /// Jump forward velocity.
        /// </summary>
        [Tooltip("Jump forward velocity.")]
        [Range(0, 20)]
        public float Speed;

        /// <summary>
        /// Character's capsule height during a jump.
        /// </summary>
        [Tooltip("Character's capsule height during a jump.")]
        [Range(0, 10)]
        public float CapsuleHeight;

        /// <summary>
        /// Duration of character's capsule height adjustment.
        /// </summary>
        [Tooltip("Duration of character's capsule height adjustment.")]
        [Range(0, 10)]
        public float HeightDuration;

        /// <summary>
        /// Default jump settings.
        /// </summary>
        public static JumpSettings Default()
        {
            JumpSettings settings;
            settings.Strength = 6;
            settings.Speed = 5;
            settings.CapsuleHeight = 1.0f;
            settings.HeightDuration = 0.75f;

            return settings;
        }
    }
}