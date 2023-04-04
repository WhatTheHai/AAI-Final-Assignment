using AAI_Final_Assignment_WinForms.Entities;

namespace AAI_Final_Assignment_WinForms.Goals.Abstracts; 

/// <summary>
///     Abstract class for a single atomic goal, cannot contain multiple goals.
/// </summary>
public abstract class AtomicGoal : Goal {
    protected AtomicGoal(Enemy entity) : base(entity) { }

    public override string Display() {
        return "  " + Name;
    }
}