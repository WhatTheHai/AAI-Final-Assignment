using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals; 

/// <summary>
///     This goal activates the seeking behaviour of a moving entity.
/// </summary>
public class SeekTargetGoal : AtomicGoal {
    private const double staminaReductionValue = 0.3;

    public SeekTargetGoal(Enemy entity) : base(entity) {
        Name = "Seeking witch";
    }

    public override void Activate() {
        Owner.SteeringBehaviour.Seek = true;
        SetActive();
    }

    public override void Process() {
        SetActiveIfInactive();
        Owner.Stamina -= staminaReductionValue;
        if (Owner.HasNoStamina() || Owner.HasLowHealth()) Deactivate();
    }

    public override void Deactivate() {
        Owner.SteeringBehaviour.Seek = false;
        SetComplete();
    }
}