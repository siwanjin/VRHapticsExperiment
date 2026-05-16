using System.Collections.Generic;
using UnityEngine;

namespace VRHapticsExperiment
{
    public class ConditionManager : MonoBehaviour
    {
        [SerializeField] private ConditionSequence sequence;

        private readonly List<ExperimentCondition> runtimeConditions = new();
        private int currentIndex = -1;

        public ExperimentCondition CurrentCondition
        {
            get
            {
                if (currentIndex < 0 || currentIndex >= runtimeConditions.Count) return null;
                return runtimeConditions[currentIndex];
            }
        }

        public int TotalCount => runtimeConditions.Count;
        public int CurrentIndex => currentIndex;

        public void Initialize()
        {
            runtimeConditions.Clear();

            if (sequence == null)
            {
                Debug.LogError("ConditionSequence is not assigned.");
                return;
            }

            runtimeConditions.AddRange(sequence.conditions);

            if (sequence.shuffleOnStart)
                Shuffle(runtimeConditions);

            currentIndex = -1;
        }

        public bool MoveNext()
        {
            currentIndex++;
            return currentIndex < runtimeConditions.Count;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = Random.Range(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}