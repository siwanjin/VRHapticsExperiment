using System.Collections;
using UnityEngine;

namespace VRHapticsExperiment
{
    public class TargetBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private Transform visualRoot;
        [SerializeField] private MeshDeformableTarget deformableTarget;
        [SerializeField] private ManualConditionController manualCondition;

        [Header("Colors")]
        [SerializeField] private Color softColor = new Color(0.25f, 0.85f, 1f);
        [SerializeField] private Color hardColor = new Color(0.75f, 0.75f, 0.75f);

        [Header("Hard Feedback")]
        [SerializeField] private float hardDuration = 0.18f;
        [SerializeField] private float hardShakeAmount = 0.18f;

        private TargetMaterialType currentType = TargetMaterialType.Soft;
        private Vector3 baseScale;
        private Vector3 baseLocalPosition;
        private Coroutine routine;

        private void Awake()
        {
            if (visualRoot == null)
                visualRoot = transform;

            if (targetRenderer == null)
                targetRenderer = visualRoot.GetComponent<Renderer>();

            if (deformableTarget == null)
                deformableTarget = visualRoot.GetComponent<MeshDeformableTarget>();

            if (manualCondition == null)
                manualCondition = FindFirstObjectByType<ManualConditionController>();

            if (visualRoot.localScale == Vector3.zero)
                visualRoot.localScale = Vector3.one;

            baseScale = visualRoot.localScale;
            baseLocalPosition = visualRoot.localPosition;
        }

        private void Start()
        {
            if (visualRoot != null)
                visualRoot.gameObject.SetActive(true);

            if (targetRenderer != null)
                targetRenderer.enabled = true;

            if (manualCondition != null)
                currentType = manualCondition.TargetMaterial;

            ApplyInitialColor();
            ResetVisualState();

            Debug.Log($"[TARGET] Start Type: {currentType}");
        }

        private void ApplyInitialColor()
        {
            if (targetRenderer == null)
                return;

            Color targetColor =
                currentType == TargetMaterialType.Soft
                    ? softColor
                    : hardColor;

            targetRenderer.material.color = targetColor;

            if (targetRenderer.material.HasProperty("_Color"))
                targetRenderer.material.SetColor("_Color", targetColor);

            if (targetRenderer.material.HasProperty("_BaseColor"))
                targetRenderer.material.SetColor("_BaseColor", targetColor);
        }

        public void ApplyMaterial(TargetMaterialType type)
        {
            currentType = type;
            ResetVisualState();

            Debug.Log($"[TARGET] ApplyMaterial Type: {currentType}");
        }

        public void PlayImpactFeedback(
            Vector3 hitPoint,
            Vector3 hitVelocity,
            TargetMaterialType materialType)
        {
            currentType = materialType;

            Debug.Log($"[TARGET] Impact Feedback: {currentType}");

            if (currentType == TargetMaterialType.Soft)
            {
                if (deformableTarget != null)
                    deformableTarget.Deform(hitPoint, hitVelocity);

                return;
            }

            if (routine != null)
                StopCoroutine(routine);

            ResetVisualState();
            routine = StartCoroutine(HardRoutine(hitVelocity));
        }

        private IEnumerator HardRoutine(Vector3 hitVelocity)
        {
            float t = 0f;

            Vector3 dir =
                hitVelocity.sqrMagnitude > 0.001f
                    ? hitVelocity.normalized
                    : Vector3.right;

            while (t < hardDuration)
            {
                t += Time.deltaTime;
                float p = 1f - (t / hardDuration);

                visualRoot.localPosition =
                    baseLocalPosition +
                    dir * Mathf.Sin(t * 160f) * hardShakeAmount * p;

                visualRoot.localScale =
                    baseScale * (1f + Mathf.Sin(t * 90f) * 0.08f * p);

                yield return null;
            }

            ResetVisualState();
            routine = null;
        }

        private void ResetVisualState()
        {
            if (visualRoot != null)
            {
                visualRoot.localScale = baseScale;
                visualRoot.localPosition = baseLocalPosition;
            }

            if (deformableTarget != null)
                deformableTarget.ResetMesh();
        }
    }
}