using System;

namespace VRHapticsExperiment
{
    public enum AttackType
    {
        Punch,
        Sword
    }

    public enum TargetMaterialType
    {
        Soft,
        Hard
    }

    public enum HapticPatternType
    {
        Simple,
        Refined
    }

    public enum ExperimentState
    {
        Idle,
        Instruction,
        ReadyCountdown,
        TrialRunning,
        Rest,
        Finished
    }
}