using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Object that calculates IK transformations.
    /// </summary>
    public class IK
    {
        /// <summary>
        /// Current object to base IK transformations on.
        /// </summary>
        public Transform Target;

        /// <summary>
        /// Chain of bones that are manipulated.
        /// </summary>
        public IKBone[] Bones;

        /// <summary>
        /// Manipulates bones till the Target is looking towards the given target position.
        /// </summary>
        /// <param name="targetPosition">Position to aim at.</param>
        /// <param name="weight">Strength of the transformations. Values range from 0 to 1.</param>
        /// <param name="iterations">Number of iterations.</param>
        public void Aim(Vector3 targetPosition, float weight, int iterations)
        {
            if (Bones.Length == 0 || weight <= float.Epsilon)
                return;

            prepareTransforms();

            for (int i = 0; i < iterations; i++)
            {
                for (int b = 0; b < Bones.Length - 1; b++)
                    solveAimBone(targetPosition, Bones[b], (i + 1) / (float)Bones.Length);

                solveAimBone(targetPosition, Bones[Bones.Length - 1], 1.0f);
            }

            assignTransforms(weight);
        }

        /// <summary>
        /// Manipulates bones till the Target is at the given target position.
        /// </summary>
        /// <param name="targetPosition">Position to move Targe to.</param>
        /// <param name="weight">Strength of the transformations. Values range from 0 to 1.</param>
        /// <param name="iterations">Number of iterations.</param>
        public void Move(Vector3 targetPosition, float weight, int iterations)
        {
            if (Bones.Length == 0 || weight <= float.Epsilon)
                return;

            prepareTransforms();

            for (int i = 0; i < iterations; i++)
            {
                for (int b = 0; b < Bones.Length - 1; b++)
                    solveMoveBone(targetPosition, Bones[b], (i + 1) / (float)Bones.Length);

                solveMoveBone(targetPosition, Bones[Bones.Length - 1], 1.0f);
            }

            assignTransforms(weight);
        }

        /// <summary>
        /// Calculates transformation of a single bone, used to aim Target to a target position.
        /// </summary>
        private void solveAimBone(Vector3 targetPosition, IKBone bone, float weightMultiplier = 1.0f)
        {
            var weight = bone.Weight * weightMultiplier;
            var offset = Quaternion.FromToRotation(Target.forward, (targetPosition - Target.position).normalized);

            if (bone.Transform != null)
                bone.Transform.rotation = Quaternion.Lerp(Quaternion.identity, offset, weight) * bone.Transform.rotation;

            if (bone.Sibling != null)
                bone.Sibling.rotation = Quaternion.Lerp(Quaternion.identity, offset, weight) * bone.Sibling.rotation;
        }

        /// <summary>
        /// Calculates transformation of a single bone, used to move Target to a target position.
        /// </summary>
        private void solveMoveBone(Vector3 targetPosition, IKBone bone, float weightMultiplier = 1.0f)
        {
            var weight = bone.Weight * weightMultiplier;
            var offset = Quaternion.FromToRotation((Target.position - bone.Transform.position).normalized, (targetPosition - bone.Transform.position).normalized);

            if (bone.Transform != null)
                bone.Transform.rotation = Quaternion.Lerp(Quaternion.identity, offset, weight) * bone.Transform.rotation;

            if (bone.Sibling != null)
                bone.Sibling.rotation = Quaternion.Lerp(Quaternion.identity, offset, weight) * bone.Sibling.rotation;
        }

        /// <summary>
        /// Remember original transformations of all bones.
        /// </summary>
        private void prepareTransforms()
        {
            for (int i = 0; i < Bones.Length; i++)
            {
                if (Bones[i].Transform != null) Bones[i].OriginalTransform = Bones[i].Transform.rotation;
                if (Bones[i].Sibling != null) Bones[i].OriginalSibling = Bones[i].Sibling.rotation;
            }
        }

        /// <summary>
        /// Assign calculated transformations to bones. 
        /// </summary>
        private void assignTransforms(float weight)
        {
            for (int i = 0; i < Bones.Length; i++)
            {
                if (Bones[i].Transform != null) Bones[i].Transform.rotation = Quaternion.Lerp(Bones[i].OriginalTransform, Bones[i].Transform.rotation, weight);
                if (Bones[i].Sibling != null) Bones[i].Sibling.rotation = Quaternion.Lerp(Bones[i].OriginalSibling, Bones[i].Sibling.rotation, weight);
            }
        }
    }
}