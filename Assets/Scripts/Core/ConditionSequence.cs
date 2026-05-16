using System.Collections.Generic;
using UnityEngine;

namespace VRHapticsExperiment
{
    [CreateAssetMenu(menuName = "VR Haptics/Condition Sequence", fileName = "ConditionSequence")]
    public class ConditionSequence : ScriptableObject
    {
        public List<ExperimentCondition> conditions = new List<ExperimentCondition>();
        public bool shuffleOnStart = true;
    }
}