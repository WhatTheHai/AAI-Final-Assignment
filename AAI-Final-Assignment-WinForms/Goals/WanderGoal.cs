using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals;

/// <summary>
///     This goal activates the wander behaviour of a moving entity. while wander it the entity regenerates stamina
/// </summary>
public class WanderGoal : AtomicGoal {
    private const double staminaIncreaseValue = 1.0;

    public WanderGoal(Enemy entity) : base(entity) {
        Name = "Wander";
    }

    public override void Activate() {
        Owner.SteeringBehaviour.Wander = true;
        SetActive();
    }

    public override void Process() {
        SetActiveIfInactive();
        Owner.Stamina += staminaIncreaseValue;
        if (Owner.HasMaxStamina() || Owner.IsToCloseToTarget(Owner.World.Witch))
            Deactivate(); // flee when to close 
    }

    public override void Deactivate() {
        Owner.SteeringBehaviour.Wander = false;
        SetComplete();
    }
}