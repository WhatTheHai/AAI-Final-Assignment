using System.Text;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Enums;


namespace AAI_Final_Assignment_WinForms.Goals.Abstracts
{
    /// <summary>
    /// Abstract class for a composite goal, can contain multiple sub goals.
    /// </summary>
    public abstract class CompositeGoal : Goal
    {
        protected Stack<Goal> SubGoalsStack { get; set; }

        protected CompositeGoal(MovingEntity entity) : base(entity)
        {
            SubGoalsStack = new Stack<Goal>();
        }

        public override void Process()
        {
            if (!IsActive()) Activate();

            // check if their is a subgoal to be processed. 
            if (SubGoalsStack.Count > 0)
            {
                var currentGoal = SubGoalsStack.Peek();

                while ((currentGoal.GoalStatus == GoalStatusType.Completed ||
                        currentGoal.GoalStatus == GoalStatusType.Failed) && SubGoalsStack.Count > 0)
                {
                    SubGoalsStack.Pop().Deactivate();
                    if (SubGoalsStack.Count > 0)
                    {
                        currentGoal = SubGoalsStack.Peek();
                    }
                }

                if (SubGoalsStack.Count <= 0) return;
                currentGoal = SubGoalsStack.Peek();
                currentGoal.Process();
            }
            else
            {
                GoalStatus = GoalStatusType.Completed;
            }
        }


        public override string Display()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + " " + GoalStatus + "\n");
            if (SubGoalsStack.Count > 0)
            {
                sb.Append(SubGoalsStack.Peek().Display());
            }

            return sb.ToString();
        }
    }
}