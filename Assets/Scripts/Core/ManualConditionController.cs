using System.Collections;
using UnityEngine;

namespace VRHapticsExperiment
{
    public class ManualConditionController : MonoBehaviour
    {
        [Header("Manual Condition")]
        [SerializeField] private string conditionId = "Manual";
        [SerializeField] private AttackType attackType = AttackType.Punch;
        [SerializeField] private TargetMaterialType targetMaterial = TargetMaterialType.Soft;
        [SerializeField] private HapticPatternType hapticPattern = HapticPatternType.Simple;

        [Header("Intensity Scale")]
        [Range(0.1f, 2f)] public float gloveIntensityScale = 1.0f;
        [Range(0.1f, 2f)] public float armIntensityScale = 1.0f;
        [Range(0.1f, 2f)] public float suitIntensityScale = 1.0f;

        [Header("References")]
        [SerializeField] private WeaponModeController weaponModeController;
        [SerializeField] private TargetBehaviour targetBehaviour;

        public string ConditionId => conditionId;
        public AttackType AttackType => attackType;
        public TargetMaterialType TargetMaterial => targetMaterial;
        public HapticPatternType HapticPattern => hapticPattern;

        private IEnumerator Start()
        {
            // TargetBehaviour.Start(), Renderer 초기화 등이 끝난 다음 적용
            yield return null;
            ApplyManualCondition();
        }

        [ContextMenu("Apply Manual Condition")]
        public void ApplyManualCondition()
        {
            conditionId = $"{attackType}_{targetMaterial}_{hapticPattern}";

            if (weaponModeController != null)
                weaponModeController.SetMode(attackType);

            if (targetBehaviour != null)
                targetBehaviour.ApplyMaterial(targetMaterial);

            Debug.Log($"[MANUAL CONDITION] {conditionId}");
        }
    }
}