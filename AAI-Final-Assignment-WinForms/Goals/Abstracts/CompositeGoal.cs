using System.Text;
using AAI_Final_Assignment_WinForms.Entities;


namespace AAI_Final_Assignment_WinForms.Goals.Abstracts
{
    /// <summary>
    /// Abstract class for a composite goal, can contain multiple sub goals.
    /// </summary>
    public abstract class CompositeGoal : Goal
    {
        protected Stack<Goal> SubGoalsStack { get; set; }

        protected CompositeGoal(string name, MovingEntity entity) : base(name, entity)
        {
            SubGoalsStack = new Stack<Goal>();
        }


        public override string Display()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + "\n");
            if (SubGoalsStack.Count > 0)
            {
                // sb.Append(SubGoalsList.First().Display());
                sb.Append(SubGoalsStack.Peek().Display());
            }

            return sb.ToString();
        }
    }
}