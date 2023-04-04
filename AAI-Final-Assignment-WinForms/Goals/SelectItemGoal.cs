using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.Goals.Enums;

namespace AAI_Final_Assignment_WinForms.Goals
{
    /// <summary>
    ///  Select a item as target as long as it exists. 
    /// </summary>
    public class SelectItemGoal : AtomicGoal
    {
        public SelectItemGoal(Enemy entity) : base(entity)
        {
            Name = "Select Item";
        }

public class SelectItemGoal : AtomicGoal {
    public SelectItemGoal(Enemy entity) : base(entity) {
        Name = "Select Item";
    }

    public override void Activate() {
        // select an item
        var itemCount = Owner.World.Items.Count;

        if (itemCount == 0) {
            GoalStatus = GoalStatusType.Failed;
        }
        else {
            SetActive();

            var random = new Random();
            Owner.CurrentTarget = Owner.World.Items[random.Next(itemCount)];
            GoalStatus = GoalStatusType.Completed;
        }
    }

    public override void Process() {
        SetActiveIfInactive();
    }

    public override void Deactivate() {
        // 
    }
}