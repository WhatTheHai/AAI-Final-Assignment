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
            SubGoalsList.Add(new SeekTargetGoal("seek", Owner));
            SubGoalsList.Add(new SeekTargetGoal("seek", Owner));
            SubGoalsList.Add(new SeekTargetGoal("seek", Owner));
            SubGoalsList.Add(new SeekTargetGoal("seek", Owner));
        }


        public override void Process()
        {
            // check if completed in list ... 

            var currentGoal = SubGoalsList.First();
        }

        public override void Deactivate()
        {
            throw new NotImplementedException();
        }


        public override void Display()
        {
            throw new NotImplementedException();
        }
    }
}