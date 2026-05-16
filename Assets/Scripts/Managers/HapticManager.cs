using UnityEngine;
using Bhaptics.SDK2;

namespace VRHapticsExperiment
{
    public class HapticManager : MonoBehaviour
    {
        [SerializeField] private bool debugLogOnly = true;

        public void PlayHitFeedback(HitContext ctx, ParticipantProfile profile, ManualConditionController condition)
        {
            float gloveScale = condition != null ? condition.gloveIntensityScale : 1.0f;
            float armScale = condition != null ? condition.armIntensityScale : 1.0f;
            float suitScale = condition != null ? condition.suitIntensityScale : 1.0f;

            float glove = Mathf.Clamp01(profile.gloveBase * gloveScale);
            float arm = Mathf.Clamp01(profile.armBase * armScale);
            float suit = Mathf.Clamp01(profile.suitBase * suitScale);

            if (ctx.attackType == AttackType.Punch)
            {
                if (ctx.targetMaterial == TargetMaterialType.Soft)
                {
                    glove *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.8f : 0.9f;
                    arm *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.5f : 0.7f;
                    suit *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.2f : 0.35f;
                }
                else
                {
                    glove *= 1.0f;
                    arm *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.8f : 0.95f;
                    suit *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.25f : 0.45f;
                }
            }
            else
            {
                if (ctx.targetMaterial == TargetMaterialType.Soft)
                {
                    glove *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.7f : 0.85f;
                    arm *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.55f : 0.8f;
                    suit *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.1f : 0.25f;
                }
                else
                {
                    glove *= 1.0f;
                    arm *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.9f : 1.0f;
                    suit *= (ctx.hapticPattern == HapticPatternType.Simple) ? 0.15f : 0.35f;
                }
            }

            string eventName = GetEventName(ctx);

            if (debugLogOnly)
            {
                Debug.Log(
                    $"[HAPTIC] {ctx.conditionId} | {ctx.attackType} | {ctx.targetMaterial} | {ctx.hapticPattern} | speed={ctx.speed:F2} => glove={glove:F2}, arm={arm:F2}, suit={suit:F2}, event={eventName}"
                );
                return;
            }

            BhapticsLibrary.Play(eventName);
        }

        private string GetEventName(HitContext ctx)
        {
            string attack = ctx.attackType == AttackType.Punch ? "punch" : "sword";
            string material = ctx.targetMaterial == TargetMaterialType.Soft ? "soft" : "hard";
            string pattern = ctx.hapticPattern == HapticPatternType.Simple ? "simple" : "refined";

            return $"{attack}_{material}_{pattern}";
        }
    }
}