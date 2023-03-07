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
            if (!SubGoalsList.Any()) Activate();

            // protected werkt niet? 
            var currentGoal = SubGoalsList.First();
            if (currentGoal.GoalStatus == GoalStatusType.Completed)
            {
                RemoveFirst();
            }

            // process first goal in list. 
            currentGoal.Process();
        }

        public override void Deactivate()
        {
        }

        public override string Display()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + "\n");
            if (SubGoalsList.Any())
            {
                sb.Append(SubGoalsList.First().Display());
            }

            return sb.ToString();
        }
    }
}