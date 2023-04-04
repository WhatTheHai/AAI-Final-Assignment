using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals;

/// <summary>
///     Healing goal, first flee away then go to a healing item
/// </summary>
public class HealGoal : CompositeGoal {
    public HealGoal(Enemy entity) : base(entity) {
        Name = "Healing";
    }

    public override void Activate() {
        SubGoalsStack.Clear();
        SubGoalsStack.Push(new MoveToItemGoal(Owner));
        SubGoalsStack.Push(new FleeGoal(Owner));
        SetActive();
    }
}