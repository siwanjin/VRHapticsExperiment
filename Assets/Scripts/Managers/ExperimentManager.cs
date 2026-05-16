using UnityEngine;

namespace VRHapticsExperiment
{
    public class ExperimentManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ManualConditionController manualCondition;
        [SerializeField] private HapticManager hapticManager;
        [SerializeField] private DataLogger dataLogger;

        [Header("Participant")]
        [SerializeField] private ParticipantProfile profile = new();

        [Header("Manual Trial")]
        [SerializeField] private bool trialRunningOnPlay = true;

        private float elapsedTrialTime;
        private int hitCount;

        public ParticipantProfile Profile => profile;
        public bool IsTrialRunning => trialRunningOnPlay;
        public float ElapsedTrialTime => elapsedTrialTime;

        public string CurrentConditionId =>
            manualCondition != null ? manualCondition.ConditionId : "Manual";

        public AttackType CurrentAttackType =>
            manualCondition != null ? manualCondition.AttackType : AttackType.Punch;

        public TargetMaterialType CurrentTargetMaterial =>
            manualCondition != null ? manualCondition.TargetMaterial : TargetMaterialType.Soft;

        public HapticPatternType CurrentHapticPattern =>
            manualCondition != null ? manualCondition.HapticPattern : HapticPatternType.Simple;

        public ManualConditionController ManualCondition => manualCondition;

        private void Start()
        {
            elapsedTrialTime = 0f;
            hitCount = 0;

            if (manualCondition != null)
                manualCondition.ApplyManualCondition();
        }

        private void Update()
        {
            if (trialRunningOnPlay)
                elapsedTrialTime += Time.deltaTime;
        }

        public void ReportHit(HitContext ctx)
        {
            hitCount++;

            if (dataLogger != null)
                dataLogger.LogHit(ctx);

            if (hapticManager != null)
                hapticManager.PlayHitFeedback(ctx, profile, manualCondition);
        }

        [ContextMenu("Reset Manual Trial")]
        public void ResetManualTrial()
        {
            elapsedTrialTime = 0f;
            hitCount = 0;
            Debug.Log("[MANUAL TRIAL] Reset");
        }

        [ContextMenu("Save Logs Now")]
        public void SaveLogsNow()
        {
            if (dataLogger != null)
                dataLogger.FlushAll();
        }
    }
}