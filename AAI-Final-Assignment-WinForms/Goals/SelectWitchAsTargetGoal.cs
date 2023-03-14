using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.Goals.Enums;

namespace AAI_Final_Assignment_WinForms.Goals
{
    public class SelectWitchAsTargetGoal : AtomicGoal
    {
        public SelectWitchAsTargetGoal(MovingEntity entity) : base(entity)
        {
            Name = "Select Witch";
        }

        public override void Activate()
        {
            GoalStatus = GoalStatusType.Active;
            Owner.CurrentTarget = Owner.World.Witch;
        }

        public override void Process()
        {
            if (!IsActive()) Activate();
            GoalStatus = GoalStatusType.Completed;
        }

        public override void Deactivate()
        {
            //
        }
    }
}