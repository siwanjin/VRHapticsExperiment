using UnityEngine;

namespace VRHapticsExperiment
{
    public class WeaponModeController : MonoBehaviour
    {
        [SerializeField] private GameObject punchObject;
        [SerializeField] private GameObject swordObject;

        public AttackType CurrentAttackType { get; private set; } = AttackType.Punch;

        public void SetMode(AttackType attackType)
        {
            CurrentAttackType = attackType;

            if (punchObject != null)
                punchObject.SetActive(attackType == AttackType.Punch);

            if (swordObject != null)
                swordObject.SetActive(attackType == AttackType.Sword);
        }
    }
}