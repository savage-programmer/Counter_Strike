using System;
using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Character's IK settings.
    /// </summary>
    [Serializable]
    public struct IKSettings
    {
        /// <summary>
        /// Defines quality of character IK.
        /// </summary>
        [Tooltip("Defines quality of character IK.")]
        [Range(1, 100)]
        public int Iterations;

        /// <summary>
        /// Chain of bones used to aim a gun towards a target.
        /// </summary>
        [Tooltip("Chain of bones used to aim a gun towards a target.\nChains go in direction away from spine.\nIntended to be filled with arm bones startin with shoulders.")]
        public IKBone[] AimBones;

        /// <summary>
        /// Chain of bones used to aim a head towards a target.
        /// </summary>
        [Tooltip("Chain of bones used to aim a head towards a target.\nChains go in direction away from spine.\nIntended to have neck as the first bone and head as the second.")]
        public IKBone[] SightBones;

        /// <summary>
        /// Chain of bones used to position a left hand on a gun.
        /// </summary>
        [Tooltip("Chain of bones used to position a left hand on a gun.\nChains go in direction away from spine.\nIntended to be filled with left arm bones starting with the left shoulder.")]
        public IKBone[] LeftArmBones;

        /// <summary>
        /// Chain of bones used to adjust right hand by recoil.
        /// </summary>
        [Tooltip("Chain of bones used to adjust right hand by recoil.\nChains go in direction away from spine.\nIntended to be filled with right arm bones starting with the left shoulder.")]
        public IKBone[] RecoilBones;

        /// <summary>
        /// Position of a left hand to maintain on a gun.
        /// </summary>
        [Tooltip("Position of a left hand to maintain on a gun.\nBones defined in the LeftBones property are adjusted till LeftHand is in the intended position.\nFor this to work LeftHand must be in the same hierarchy as those bones.")]
        public Transform LeftHand;

        /// <summary>
        /// Position of a right hand to adjust by recoil.
        /// </summary>
        [Tooltip("Position of a right hand to adjust by recoil.\nBones defined in the RightBones property are adjusted till RightHand is in the intended position.\nFor this to work RightHand must be in the same hierarchy as those bones.")]
        public Transform RightHand;

        /// <summary>
        /// Bone to adjust when a character is hit.
        /// </summary>
        [Tooltip("Bone to adjust when a character is hit.")]
        public Transform HitBone;

        /// <summary>
        /// Transform to manipulate so it is facing towards a target. Used when aiming a head.
        /// </summary>
        [Tooltip("Transform to manipulate so it is facing towards a target. Used when aiming a head.\nBones defined in the LookBones are modified till Look is pointing at the intended direction.\nTherefore it Look must be in the same hierarchy as thsoe bones.")]
        public Transform Sight;

        /// <summary>
        /// Default IK settings.
        /// </summary>
        public static IKSettings Default()
        {
            var settings = new IKSettings();
            settings.Iterations = 2;

            return settings;
        }
    }

    /// <summary>
    /// Settings of a bone to be manipulated by IK.
    /// </summary>
    [Serializable]
    public struct IKBone
    {
        /// <summary>
        /// Link to the bone.
        /// </summary>
        [Tooltip("Link to the bone.")]
        public Transform Transform;

        /// <summary>
        /// Link to the bone to be adjusted the same way as Transform. Used for left arm which is adjusted the same as the right.
        /// </summary>
        [Tooltip("Link to the bone to be adjusted the same way as Transform. Used for left arm which is adjusted the same as the right.")]
        public Transform Sibling;

        /// <summary>
        /// Defines bone's influence in a bone chain.
        /// </summary>
        [Tooltip("Defines bone's influence in a bone chain.")]
        [Range(0, 1)]
        public float Weight;

        /// <summary>
        /// Used to save original rotation before calculating IK.
        /// </summary>
        [HideInInspector]
        internal Quaternion OriginalTransform;

        /// <summary>
        /// Used to save original rotation of a sibling before calculating IK.
        /// </summary>
        [HideInInspector]
        internal Quaternion OriginalSibling;

        public IKBone(Transform transform, float weight)
        {
            Transform = transform;
            Sibling = null;
            Weight = weight;
            OriginalTransform = Quaternion.identity;
            OriginalSibling = Quaternion.identity;
        }

        public IKBone(Transform transform, Transform sibling, float weight)
        {
            Transform = transform;
            Sibling = sibling;
            Weight = weight;
            OriginalTransform = Quaternion.identity;
            OriginalSibling = Quaternion.identity;
        }
    }
}