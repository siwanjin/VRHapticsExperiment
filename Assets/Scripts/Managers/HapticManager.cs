using UnityEngine;
using Bhaptics.SDK2;

namespace VRHapticsExperiment
{
    public class HapticManager : MonoBehaviour
    {
        [Header("Playback")]
        [SerializeField] private bool enableHaptics = true;
        [SerializeField] private bool debugLogOnly = false;

        public void PlayHitFeedback(HitContext ctx, ParticipantProfile profile, ManualConditionController condition)
        {
            string eventName = GetEventName(ctx);

            Debug.Log(
                $"[HAPTIC] condition={ctx.conditionId}, attack={ctx.attackType}, material={ctx.targetMaterial}, pattern={ctx.hapticPattern}, event={eventName}, speed={ctx.speed:F2}"
            );

            if (!enableHaptics)
                return;

            if (debugLogOnly)
                return;

            BhapticsLibrary.Play(eventName);
        }

        private string GetEventName(HitContext ctx)
        {
            string attack =
                ctx.attackType == AttackType.Punch ? "punch" : "sword";

            string material =
                ctx.targetMaterial == TargetMaterialType.Soft ? "soft" : "hard";

            string pattern =
                ctx.hapticPattern == HapticPatternType.Simple ? "simple" : "refined";

            return $"{attack}_{material}_{pattern}";
        }

        [ContextMenu("Test Punch Soft Simple")]
        private void TestPunchSoftSimple()
        {
            BhapticsLibrary.Play("punch_soft_simple");
        }

        [ContextMenu("Test Punch Soft Refined")]
        private void TestPunchSoftRefined()
        {
            BhapticsLibrary.Play("punch_soft_refined");
        }

        [ContextMenu("Test Punch Hard Simple")]
        private void TestPunchHardSimple()
        {
            BhapticsLibrary.Play("punch_hard_simple");
        }

        [ContextMenu("Test Punch Hard Refined")]
        private void TestPunchHardRefined()
        {
            BhapticsLibrary.Play("punch_hard_refined");
        }

        [ContextMenu("Test Sword Soft Simple")]
        private void TestSwordSoftSimple()
        {
            BhapticsLibrary.Play("sword_soft_simple");
        }

        [ContextMenu("Test Sword Soft Refined")]
        private void TestSwordSoftRefined()
        {
            BhapticsLibrary.Play("sword_soft_refined");
        }

        [ContextMenu("Test Sword Hard Simple")]
        private void TestSwordHardSimple()
        {
            BhapticsLibrary.Play("sword_hard_simple");
        }

        [ContextMenu("Test Sword Hard Refined")]
        private void TestSwordHardRefined()
        {
            BhapticsLibrary.Play("sword_hard_refined");
        }
    }
}