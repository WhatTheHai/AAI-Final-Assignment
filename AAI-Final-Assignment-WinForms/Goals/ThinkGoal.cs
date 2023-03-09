using System.Text;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.Goals.Enums;


namespace AAI_Final_Assignment_WinForms.Goals
{
    public class ThinkGoal : CompositeGoal
    {
        public ThinkGoal(string name, MovingEntity entity) : base(name, entity)
        {
        }

        public override void Activate()
        {
            GoalStatus = GoalStatusType.Active;
            Add(new WanderGoal("Wander", Owner));
            Add(new SeekTargetGoal("Seek", Owner));
            Add(new WanderGoal("Wander", Owner));
            Add(new SeekTargetGoal("Seek", Owner));
        }


        public override void Process()
        {
            // check if completed in list ... also remove failed  
            //if (!SubGoalsList.Any()) Activate();
            if (SubGoalsStack.Count == 0) Activate();

            // var currentGoal = SubGoalsList.First();
            var currentGoal = SubGoalsStack.Peek();

            if (currentGoal.GoalStatus == GoalStatusType.Completed || currentGoal.GoalStatus == GoalStatusType.Failed)
            {
                Remove();
            }

            // process first goal in list. 
            currentGoal.Process();
        }

        public override void Deactivate()
        {
        }
    }
}