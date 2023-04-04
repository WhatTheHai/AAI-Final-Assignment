using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals
{
    /// <summary>
    /// flee away from attacker. accounts for flee distance set in enemy
    /// </summary>
    public class FleeGoal : AtomicGoal
    {
        public FleeGoal(Enemy entity) : base(entity)
        {
            Name = "Fleeing";
        }

public class FleeGoal : AtomicGoal {
    public FleeGoal(Enemy entity) : base(entity) {
        Name = "Fleeing";
    }

    public override void Activate() {
        Owner.SteeringBehaviour.Flee = true;
        SetActive();
    }

    public override void Process() {
        SetActiveIfInactive();
        if (!Owner.IsToCloseToTarget(Owner.World.Witch)) Deactivate();
    }

    public override void Deactivate() {
        Owner.SteeringBehaviour.Flee = false;
        SetComplete();
    }
}