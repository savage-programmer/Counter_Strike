using UnityEditor;
using UnityEngine;

namespace ThirdPersonCover
{
    [CustomEditor(typeof(CharacterMotor))]
    [CanEditMultipleObjects]
    public class CharacterMotorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Auto IK Setup"))
            {
                Undo.RecordObjects(targets, "IK Setup");

                foreach (var object_ in targets)
                {
                    var motor = (CharacterMotor)object_;
                    var animator = motor.GetComponent<Animator>();

                    var settings = motor.IK;

                    var spine = animator.GetBoneTransform(HumanBodyBones.Spine);
                    var lowerSpine = spine.GetChild(0);

                    settings.AimBones = new IKBone[3];
                    settings.AimBones[0] = new IKBone(spine, 0.5f);
                    settings.AimBones[1] = new IKBone(lowerSpine, 0.5f);
                    settings.AimBones[2] = new IKBone(animator.GetBoneTransform(HumanBodyBones.RightShoulder), animator.GetBoneTransform(HumanBodyBones.LeftShoulder), 0.8f);

                    settings.SightBones = new IKBone[2];
                    settings.SightBones[0] = new IKBone(animator.GetBoneTransform(HumanBodyBones.Neck), 0.5f);
                    settings.SightBones[1] = new IKBone(animator.GetBoneTransform(HumanBodyBones.Head), 0.8f);

                    settings.LeftArmBones = new IKBone[3];
                    settings.LeftArmBones[0] = new IKBone(animator.GetBoneTransform(HumanBodyBones.LeftShoulder), 0.5f);
                    settings.LeftArmBones[1] = new IKBone(animator.GetBoneTransform(HumanBodyBones.LeftUpperArm), 0.5f);
                    settings.LeftArmBones[2] = new IKBone(animator.GetBoneTransform(HumanBodyBones.LeftLowerArm), 0.8f);

                    settings.RecoilBones = new IKBone[4];
                    settings.RecoilBones[0] = new IKBone(lowerSpine, 0.25f);
                    settings.RecoilBones[1] = new IKBone(animator.GetBoneTransform(HumanBodyBones.RightShoulder), 0.5f);
                    settings.RecoilBones[2] = new IKBone(animator.GetBoneTransform(HumanBodyBones.RightUpperArm), 0.8f);
                    settings.RecoilBones[3] = new IKBone(animator.GetBoneTransform(HumanBodyBones.RightLowerArm), 0.5f);

                    settings.LeftHand = animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal);
                    settings.RightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
                    settings.HitBone = spine;

                    motor.IK = settings;
                }
            }
        }
    }
}
