using UnityEngine;

namespace VRHapticsExperiment
{
    public struct HitContext
    {
        public string participantId;
        public string conditionId;
        public AttackType attackType;
        public TargetMaterialType targetMaterial;
        public HapticPatternType hapticPattern;
        public string targetName;
        public Vector3 hitPoint;
        public Vector3 weaponVelocity;
        public float speed;
        public float trialTime;
        public bool isValidHit;

        public HitContext(
            string participantId,
            string conditionId,
            AttackType attackType,
            TargetMaterialType targetMaterial,
            HapticPatternType hapticPattern,
            string targetName,
            Vector3 hitPoint,
            Vector3 weaponVelocity,
            float speed,
            float trialTime,
            bool isValidHit)
        {
            this.participantId = participantId;
            this.conditionId = conditionId;
            this.attackType = attackType;
            this.targetMaterial = targetMaterial;
            this.hapticPattern = hapticPattern;
            this.targetName = targetName;
            this.hitPoint = hitPoint;
            this.weaponVelocity = weaponVelocity;
            this.speed = speed;
            this.trialTime = trialTime;
            this.isValidHit = isValidHit;
        }
    }
}