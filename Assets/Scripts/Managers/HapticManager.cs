using UnityEngine;
using Bhaptics.SDK2;

namespace VRHapticsExperiment
{
    public class HapticManager : MonoBehaviour
    {
        [Header("Playback")]
        [SerializeField] private bool enableHaptics = true;

        public void PlayHitFeedback(HitContext ctx, ParticipantProfile profile, ManualConditionController condition)
        {
            if (!enableHaptics)
            {
                Debug.LogWarning("[HAPTIC] Blocked: enableHaptics is false");
                return;
            }

            string eventName = GetEventName(ctx);

            Debug.Log(
                $"[HAPTIC] Play requested: {eventName} | " +
                $"attack={ctx.attackType}, material={ctx.targetMaterial}, pattern={ctx.hapticPattern}"
            );

            BhapticsLibrary.Play(eventName);

            Debug.Log("[HAPTIC] BhapticsLibrary.Play called: " + eventName);
        }

        private string GetEventName(HitContext ctx)
        {
            string attack = ctx.attackType switch
            {
                AttackType.Punch => "punch",
                AttackType.Sword => "sword",
                _ => "punch"
            };

            string material = ctx.targetMaterial switch
            {
                TargetMaterialType.Soft => "soft",
                TargetMaterialType.Hard => "hard",
                _ => "soft"
            };

            string pattern = ctx.hapticPattern switch
            {
                HapticPatternType.Simple => "simple",
                HapticPatternType.Refined => "refined",
                _ => "simple"
            };

            return $"{attack}_{material}_{pattern}";
        }

        [ContextMenu("TEST Current Default Pattern")]
        private void TestDefault()
        {
            BhapticsLibrary.Play("punch_soft_simple");
            Debug.Log("[HAPTIC TEST] punch_soft_simple");
        }
    }
}
