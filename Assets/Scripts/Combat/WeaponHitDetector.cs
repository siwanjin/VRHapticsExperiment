using UnityEngine;

namespace VRHapticsExperiment
{
    [RequireComponent(typeof(Collider))]
    public class WeaponHitDetector : MonoBehaviour
    {
        [SerializeField] private ExperimentManager experimentManager;
        [SerializeField] private Transform trackedTransform;
        [SerializeField] private float minHitSpeed = 0.75f;
        [SerializeField] private float sameTargetCooldown = 0.15f;

        private Vector3 previousPosition;
        private Vector3 currentVelocity;
        private float lastHitTime = -999f;
        private Collider lastTarget;

        private void Start()
        {
            if (trackedTransform == null)
                trackedTransform = transform;

            previousPosition = trackedTransform.position;
        }

        private void Update()
        {
            Vector3 currentPosition = trackedTransform.position;

            currentVelocity =
                (currentPosition - previousPosition) /
                Mathf.Max(Time.deltaTime, 0.0001f);

            previousPosition = currentPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            TryHit(other);
        }

        private void TryHit(Collider other)
        {
            if (experimentManager == null || !experimentManager.IsTrialRunning)
                return;

            if (other == lastTarget && Time.time - lastHitTime < sameTargetCooldown)
                return;

            TargetBehaviour target = other.GetComponentInParent<TargetBehaviour>();
            if (target == null)
                return;

            float speed = currentVelocity.magnitude;
            if (speed < minHitSpeed)
                return;

            Vector3 hitPoint = other.ClosestPoint(transform.position);

            Vector3 targetToHit =
                (hitPoint - target.transform.position).sqrMagnitude > 0.0001f
                    ? (hitPoint - target.transform.position).normalized
                    : Vector3.zero;

            Vector3 velocityDir = currentVelocity.normalized;

            float exitDot = Vector3.Dot(velocityDir, targetToHit);

            if (exitDot > 0.2f)
                return;

            TargetMaterialType materialType = experimentManager.CurrentTargetMaterial;

            target.PlayImpactFeedback(
                hitPoint,
                currentVelocity,
                materialType
            );

            lastHitTime = Time.time;
            lastTarget = other;

            HitContext ctx = new HitContext(
                experimentManager.Profile.participantId,
                experimentManager.CurrentConditionId,
                experimentManager.CurrentAttackType,
                materialType,
                experimentManager.CurrentHapticPattern,
                target.name,
                hitPoint,
                currentVelocity,
                speed,
                experimentManager.ElapsedTrialTime,
                true
            );

            experimentManager.ReportHit(ctx);
        }
    }
}