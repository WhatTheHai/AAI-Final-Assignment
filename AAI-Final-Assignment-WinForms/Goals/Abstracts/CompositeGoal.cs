using System.Text;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Enums;

namespace AAI_Final_Assignment_WinForms.Goals.Abstracts;

/// <summary>
///     Abstract class for a composite goal, can contain multiple sub goals.
/// </summary>
public abstract class CompositeGoal : Goal {
    protected CompositeGoal(Enemy entity) : base(entity) {
        SubGoalsStack = new Stack<Goal>();
    }

    protected Stack<Goal> SubGoalsStack { get; set; }

    public override void Process() {
        SetActiveIfInactive();

        if (SubGoalsStack.Count > 0)
            ProcessStack();
        else
            Deactivate();
    }

    protected void ProcessStack() {
        // extra checks for stack and a current goal that is null because of a null exception sometimes
        var currentGoal = SubGoalsStack.Peek();
        while (
            SubGoalsStack.Count > 0 && SubGoalsStack.Peek() != null && currentGoal != null &&
            (currentGoal.GoalStatus == GoalStatusType.Completed || currentGoal.GoalStatus == GoalStatusType.Failed)) {
            if (SubGoalsStack.Count > 0) SubGoalsStack.Pop();
            if (SubGoalsStack.Count > 0) currentGoal = SubGoalsStack.Peek();
        }

        if (SubGoalsStack.Count > 0 && currentGoal != null) currentGoal.Process();
    }

    public override void Deactivate() {
        SetComplete();
    }

    public override string Display() {
        var sb = new StringBuilder();
        sb.Append(Name + "\n");
        if (SubGoalsStack.Count > 0)
            if (SubGoalsStack.Peek() != null)
                sb.Append("   " + SubGoalsStack.Peek().Display());

        return sb.ToString();
    }
}