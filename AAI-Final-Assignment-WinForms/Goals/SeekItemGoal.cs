using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.Goals.Enums;

namespace AAI_Final_Assignment_WinForms.Goals; 

/// <summary>
///     This goal activates the seeking behaviour of a moving entity.
/// </summary>
public class SeekItemGoal : AtomicGoal {
    public SeekItemGoal(Enemy entity) : base(entity) {
        Name = "Seeking item";
    }

    public override void Activate() {
        Owner.SteeringBehaviour.Seek = true;
        SetActive();
    }


    public override void Process() {
        SetActiveIfInactive();
        if (!Owner.World.Items.Contains(Owner.CurrentTarget))
            GoalStatus = GoalStatusType.Failed;
    }

    public override void Deactivate() {
        Owner.SteeringBehaviour.Seek = false;
        SetComplete();
    }
}