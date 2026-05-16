using System.Collections;
using UnityEngine;

namespace VRHapticsExperiment
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshDeformableTarget : MonoBehaviour
    {
        [Header("Local Dent")]
        [SerializeField] private float radius = 0.35f;
        [SerializeField] private float depth = 0.18f;
        [SerializeField] private float duration = 0.18f;

        [Header("Elastic Feel")]
        [SerializeField] private float overshoot = 0.04f;

        private Mesh deformMesh;
        private Vector3[] originalVertices;
        private Vector3[] workingVertices;
        private Coroutine dentRoutine;

        private void Awake()
        {
            deformMesh = GetComponent<MeshFilter>().mesh;
            originalVertices = deformMesh.vertices;
            workingVertices = deformMesh.vertices.Clone() as Vector3[];
        }

        public void Deform(Vector3 worldHitPoint, Vector3 hitVelocity)
        {
            if (hitVelocity.sqrMagnitude < 0.001f)
                return;

            if (dentRoutine != null)
                StopCoroutine(dentRoutine);

            dentRoutine = StartCoroutine(DentRoutine(worldHitPoint, hitVelocity.normalized));
        }

        private IEnumerator DentRoutine(Vector3 worldHitPoint, Vector3 worldHitDir)
        {
            Vector3 localHitPoint = transform.InverseTransformPoint(worldHitPoint);
            Vector3 localHitDir = transform.InverseTransformDirection(worldHitDir).normalized;

            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float x = Mathf.Clamp01(t / duration);

                float dentAmount = Mathf.Sin(x * Mathf.PI) * depth;

                float reboundAmount = 0f;
                if (x > 0.65f)
                {
                    float r = (x - 0.65f) / 0.35f;
                    reboundAmount = Mathf.Sin(r * Mathf.PI) * overshoot;
                }

                for (int i = 0; i < workingVertices.Length; i++)
                {
                    Vector3 v = originalVertices[i];

                    float dist = Vector3.Distance(v, localHitPoint);
                    if (dist > radius)
                    {
                        workingVertices[i] = v;
                        continue;
                    }

                    float falloff = 1f - (dist / radius);
                    falloff = falloff * falloff * (3f - 2f * falloff); // smoothstep

                    Vector3 offset = (localHitDir * dentAmount - localHitDir * reboundAmount) * falloff;

                    workingVertices[i] = v + offset;
                }

                deformMesh.vertices = workingVertices;
                deformMesh.RecalculateNormals();
                deformMesh.RecalculateBounds();

                yield return null;
            }

            ResetMesh();
            dentRoutine = null;
        }

        public void ResetMesh()
        {
            if (originalVertices == null || workingVertices == null)
                return;

            for (int i = 0; i < workingVertices.Length; i++)
                workingVertices[i] = originalVertices[i];

            deformMesh.vertices = workingVertices;
            deformMesh.RecalculateNormals();
            deformMesh.RecalculateBounds();
        }
    }
}