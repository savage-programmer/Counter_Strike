using System;
using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Defines cover settings for a character.
    /// </summary>
    [Serializable]
    public struct CoverSettings
    {
		public LayerMask Cover;
        /// <summary>
        /// Determines if the character takes cover automatically instead of waiting for player input.
        /// </summary>
        [Tooltip("Determines if the character takes cover automatically instead of waiting for player input.")]
        public bool IsAuto;

        /// <summary>
        /// Height of character's collision capsule when in low cover.
        /// </summary>
        [Tooltip("Height of character's collision capsule when in low cover.")]
        [Range(0, 10)]
        public float LowCapsuleHeight;

        /// <summary>
        /// Minimal distance from characters feet to the top of a cover for it to be considered tall.
        /// </summary>
        [Tooltip("Minimal distance from characters feet to the top of a cover for it to be considered tall.")]
        [Range(0, 5)]
        public float TallThreshold;

        /// <summary>
        /// How quickly the character is orientated towards a direction.
        /// </summary>
        [Tooltip("How quickly the character is orientated towards a direction.")]
        [Range(0, 50)]
        public float RotationSpeed;

        /// <summary>
        /// Character enter cover if it is closer than this value. Defined as a distance between a cover and an edge of players capsule.
        /// </summary>
        [Tooltip("Character enter cover if it is closer than this value. Defined as a distance between a cover and an edge of players capsule.")]
        [Range(0, 10)]
        public float EnterDistance;

        /// <summary>
        /// Character exit cover if it is furhter away than this value. Defined as a distance between a cover and an edge of players capsule.
        /// </summary>
        [Tooltip("Character exit cover if it is furhter away than this value. Defined as a distance between a cover and an edge of players capsule.")]
        [Range(0, 10)]
        public float LeaveDistance;

        /// <summary>
        /// Controls the location of camera pivot position when in cover. Pivot point does not move beyond this margin.
        /// </summary>
        [Tooltip("Controls the location of camera pivot position when in cover. Pivot point does not move beyond this margin.")]
        [Range(-10, 10)]
        public float PivotSideMargin;

        /// <summary>
        /// Distance from a side of a cover at which player can enter aiming from a corner.
        /// </summary>
        [Tooltip("Distance from a side of a cover at which player can enter aiming from a corner.")]
        [Range(0, 10)]
        public float CornerAimTriggerDistance;

        /// <summary>
        /// Capsule radius used when determining if the character is in front of a tall cover.
        /// </summary>
        [Tooltip("Capsule radius used when determining if the character is in front of a tall cover.")]
        [Range(0, 10)]
        public float TallSideRadius;

        /// <summary>
        /// Capsule radius used when determining if the character is in front of a low cover.
        /// </summary>
        [Tooltip("Capsule radius used when determining if the character is in front of a low cover.")]
        [Range(0, 10)]
        public float LowSideRadius;

        /// <summary>
        /// Time in seconds for player to start moving again after changing direction.
        /// </summary>
        [Tooltip("Time in seconds for player to start moving again after changing direction.")]
        [Range(0, 5)]
        public float DirectionChangeDelay;

        /// <summary>
        /// Time in seconds for the character to take cover automatically when not facing the front of that cover. 
        /// Characters take cover immediately if they approach it when facing it.
        /// </summary>
        [Tooltip("Time in seconds for the character to take cover automatically when not facing the front of that cover. ")]
        [Range(0, 5)]
        public float BackDelay;

        /// <summary>
        /// Defines angles for various gameplay situations. 
        /// Side axis is defined as a line going from one corner of a cover to another. 
        /// Front axis is going from a character towards the cover.
        /// </summary>
        [Tooltip("Defines angles for various gameplay situations. ")]
        public CoverAngleSettings Angles;

        /// <summary>
        /// Default cover settings.
        /// </summary>
        public static CoverSettings Default()
        {
            var settings = new CoverSettings();
            settings.IsAuto = true;
            settings.TallThreshold = 1.1f;
            settings.LowCapsuleHeight = 0.75f;
            settings.RotationSpeed = 20.0f;
            settings.EnterDistance = 0.15f;
            settings.LeaveDistance = 0.25f;
            settings.PivotSideMargin = 0.5f;
            settings.CornerAimTriggerDistance = 0.6f;
            settings.TallSideRadius = 0.3f;
            settings.LowSideRadius = 0.5f;
            settings.DirectionChangeDelay = 0.25f;
            settings.BackDelay = 0.5f;
            settings.Angles = CoverAngleSettings.Default();

            return settings;
        }
    }

    [Serializable]
    public struct CoverAngleSettings
    {
        /// <summary>
        /// Front area of a cover in angles, defined as a circle.
        /// </summary>
        [Tooltip("Front area of a cover in angles, defined as a circle.")]
        [Range(0, 360)]
        public float Front;

        /// <summary>
        /// Angle from side axis to exit back aiming form a tall cover when near a corner.
        /// </summary>
        [Tooltip("Angle from side axis to exit back aiming form a tall cover when near a corner.")]
        [Range(-90, 90)]
        public float CornerBackAimLeave;

        /// <summary>
        /// Angle from side axis opposite from a facing direction that sustains previous facing direction even if player is moving opposite of it.
        /// </summary>
        [Tooltip("Angle from side axis opposite from a facing direction that sustains previous facing direction even if player is moving opposite of it.")]
        [Range(-90, 90)]
        public float LowWalkFaceChange;

        /// <summary>
        /// Angles from side axis to trigger aiming away from a tall cover.
        /// </summary>
        [Tooltip("Angle from side axis to trigger aiming away from a tall cover.")]
        public FieldAnglesSustain TallBack;

        /// <summary>
        /// Angles from side axis that lower the character, used when aiming from a lower cover.
        /// </summary>
        [Tooltip("Angle from side axis that lowers the character, used when aiming from a lower cover.")]
        public SideAngles LowerAim;

        /// <summary>
        /// Angles from front axis that manage left corner aiming state.
        /// </summary>
        [Tooltip("Angle from front axis that manages left corner aiming state.")]
        public TriggerAngles LeftCorner;

        /// <summary>
        /// Angles from front axis that manage right corner aiming state.
        /// </summary>
        [Tooltip("Angle from front axis that manages right corner aiming state.")]
        public TriggerAngles RightCorner;

        /// <summary>
        /// Angles from the front axis used to maintain a facing direction when aiming.
        /// </summary>
        [Tooltip("Angle from front axis used to maintain a facing direction when aiming.")]
        public SideAngles LowAimFaceChange;

        /// <summary>
        /// Angles from side axis that allow player to fire at a tall cover wall.
        /// </summary>
        [Tooltip("Angle from side axis that allows player to fire at a tall cover wall.")]
        public FaceAngles TallWallAim;

        /// <summary>
        /// Default angle settings for a character.
        /// </summary>
        public static CoverAngleSettings Default()
        {
            var settings = new CoverAngleSettings();
            settings.Front = 140;
            settings.LowWalkFaceChange = 60;
            settings.CornerBackAimLeave = 40;
            settings.TallBack = new FieldAnglesSustain(20, 30, 1.0f);
            settings.LowerAim = new SideAngles(-5, 10);
            settings.LeftCorner = new TriggerAngles(-15, -17);
            settings.RightCorner = new TriggerAngles(-25, -27);
            settings.LowAimFaceChange = new SideAngles(0, 20);
            settings.TallWallAim = new FaceAngles(70, 40);

            return settings;
        }
    }

    /// <summary>
    /// Angles that define enter and exit parameters for a state.
    /// </summary>
    [Serializable]
    public struct TriggerAngles
    {
        /// <summary>
        /// Degrees from an axis to trigger a state.
        /// </summary>
        [Tooltip("Degrees from the front axis to trigger a state.")]
        [Range(-90, 90)]
        public float Enter;

        /// <summary>
        /// Degrees from an axis to trigger an exit from a state.
        /// </summary>
        [Tooltip("Degrees from the front axis to trigger an exit from a state.")]
        [Range(-90, 90)]
        public float Exit;

        public TriggerAngles(float enter, float exit)
        {
            Enter = enter;
            Exit = exit;
        }
    }

    /// <summary>
    /// Defines a field that is orientated by character face.
    /// </summary>
    [Serializable]
    public struct FaceAngles
    {
        /// <summary>
        /// Degrees from an axis when facing the same direction as the player.
        /// </summary>
        [Tooltip("Degrees from a side axis when facing the same direction as the player.")]
        [Range(-90, 90)]
        public float Face;

        /// <summary>
        /// Degrees from an axis when facing the opposite direction as the player.
        /// </summary>
        [Tooltip("Degrees from a side axis when facing the opposite direction as the player.")]
        [Range(-90, 90)]
        public float Opposite;

        public FaceAngles(float face, float opposite)
        {
            Face = face;
            Opposite = opposite;
        }
    }

    /// <summary>
    /// Defines a field that is orientated by character face with a sustain time.
    /// </summary>
    [Serializable]
    public struct FieldAnglesSustain
    {
        /// <summary>
        /// Degrees from an axis when facing the same direction as the player.
        /// </summary>
        [Tooltip("Degrees from a side axis when facing the same direction as the player.")]
        [Range(-90, 90)]
        public float Face;

        /// <summary>
        /// Degrees from an axis when facing the opposite direction as the player.
        /// </summary>
        [Tooltip("Degrees from a side axis when facing the opposite direction as the player.")]
        [Range(-90, 90)]
        public float Opposite;

        /// <summary>
        /// Time in seconds to sustain a change to the opposite direction.
        /// </summary>
        [Tooltip("Time in seconds to sustain a change to the opposite direction.")]
        [Range(0, 10)]
        public float OppositeSustainTime;

        public FieldAnglesSustain(float face, float opposite, float sustain)
        {
            Face = face;
            Opposite = opposite;
            OppositeSustainTime = sustain;
        }
    }

    /// <summary>
    /// Defines angles for left and right sides.
    /// </summary>
    [Serializable]
    public struct SideAngles
    {
        /// <summary>
        /// Degrees from an axis when facing left of the cover.
        /// </summary>
        [Tooltip("Degrees from a side axis when facing left of the cover.")]
        [Range(0, 90)]
        public float Left;

        /// <summary>
        /// Degrees from an axis when facing right of the cover.
        /// </summary>
        [Tooltip("Degrees from a side axis when facing right of the cover.")]
        [Range(0, 90)]
        public float Right;

        public SideAngles(float left, float right)
        {
            Left = left;
            Right = right;
        }
    }
}