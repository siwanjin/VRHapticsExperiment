using UnityEngine;

namespace VRHapticsExperiment
{
    [CreateAssetMenu(menuName = "VR Haptics/Experiment Condition", fileName = "Condition_")]
    public class ExperimentCondition : ScriptableObject
    {
        public string conditionId = "C01";
        public AttackType attackType = AttackType.Punch;
        public TargetMaterialType targetMaterial = TargetMaterialType.Soft;
        public HapticPatternType hapticPattern = HapticPatternType.Simple;

        [Header("Intensity Scale")]
        [Range(0.1f, 2f)] public float gloveIntensityScale = 1.0f;
        [Range(0.1f, 2f)] public float armIntensityScale = 1.0f;
        [Range(0.1f, 2f)] public float suitIntensityScale = 1.0f;

        [Header("Timing")]
        public float instructionDuration = 2f;
        public float readyCountdown = 3f;
        public float trialDuration = 20f;
        public float restDuration = 5f;

        [Header("Target")]
        public GameObject targetPrefab;
    }
}