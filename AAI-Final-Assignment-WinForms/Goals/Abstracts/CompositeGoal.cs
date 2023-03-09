using System.Text;
using AAI_Final_Assignment_WinForms.Entities;


namespace AAI_Final_Assignment_WinForms.Goals.Abstracts
{
    public abstract class CompositeGoal : Goal
    {
        protected Stack<Goal> SubGoalsStack { get; set; }

        protected CompositeGoal(string name, MovingEntity entity) : base(name, entity)
        {
            SubGoalsStack = new Stack<Goal>();
        }

        public override void Add(Goal goal)
        {
            SubGoalsStack.Push(goal);
        }

        public override void Remove()
        {
            SubGoalsStack.Pop();
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