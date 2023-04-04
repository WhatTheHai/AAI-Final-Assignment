using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals; 

public class MoveToItemGoal : CompositeGoal {
    public MoveToItemGoal(Enemy entity) : base(entity) {
        Name = "MovingToItem";
    }

    public override void Activate() {
        SetActive();
        SubGoalsStack.Clear();
        SubGoalsStack.Push(new SeekItemGoal(Owner));
        SubGoalsStack.Push(new SelectItemGoal(Owner));
    }
}