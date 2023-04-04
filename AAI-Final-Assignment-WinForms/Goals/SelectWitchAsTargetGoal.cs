using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals; 

/// <summary>
///     set the witch as target.
/// </summary>
public class SelectWitchAsTargetGoal : AtomicGoal {
    public SelectWitchAsTargetGoal(Enemy entity) : base(entity) {
        Name = "Select Witch";
    }

    public override void Activate() {
        Owner.CurrentTarget = Owner.World.Witch;
        SetActive();
    }

    public override void Process() {
        SetActiveIfInactive();
        SetComplete();
    }

    public override void Deactivate() {
        // Has no deactivate
    }
}