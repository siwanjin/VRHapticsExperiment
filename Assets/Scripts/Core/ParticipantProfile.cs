using System;
using UnityEngine;

namespace VRHapticsExperiment
{
    [Serializable]
    public class ParticipantProfile
    {
        public string participantId = "P001";

        [Header("Calibration Base Intensities")]
        [Range(0f, 1f)] public float gloveBase = 0.6f;
        [Range(0f, 1f)] public float armBase = 0.5f;
        [Range(0f, 1f)] public float suitBase = 0.4f;
    }
}