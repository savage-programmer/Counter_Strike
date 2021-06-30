using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Movement information of a character.
    /// </summary>
    public struct CharacterMovement
    {
        /// <summary>
        /// Direction multiplier by Magnitude.
        /// </summary>
        public Vector3 Value { get { return Direction * Magnitude; } }

        /// <summary>
        /// Is there any movement.
        /// </summary>
        public bool IsMoving { get { return Value.sqrMagnitude > 0.1f; } }

        /// <summary>
        /// Is Magnitude great enough for the character to be running.
        /// </summary>
        public bool IsRunning { get { return Magnitude > 0.6f & IsMoving; } }

        /// <summary>
        /// Direction to move to in world space.
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// Speed of movement. 1 is running, 0.5 is walking.
        /// </summary>
        public float Magnitude;

        /// <summary>
        /// Creates a character movement description.
        /// </summary>
        /// <param name="direction">Direction to move to in world space.</param>
        /// <param name="magnitude">Creates a character movement description.</param>
        public CharacterMovement(Vector3 direction, float magnitude)
        {
            Direction = direction;
            Magnitude = magnitude;
        }
    }
}