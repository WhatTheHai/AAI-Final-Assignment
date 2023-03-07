using AAI_Final_Assignment_WinForms.Entities;


namespace AAI_Final_Assignment_WinForms.Goals.Abstracts
{
    public abstract class CompositeGoal : Goal
    {
        protected List<Goal> SubGoalsList { get; set; }

        protected CompositeGoal(string name, MovingEntity entity) : base(name, entity)
        {
            SubGoalsList = new List<Goal>();
        }

        public override void Add(Goal goal)
        {
            // todo: add first? 
            //  SubGoalsList.Add(goal);
            SubGoalsList.Insert(0, goal);
        }
    }
}