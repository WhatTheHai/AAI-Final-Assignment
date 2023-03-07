using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Enums;

namespace AAI_Final_Assignment_WinForms.Goals.Abstracts
{
    public abstract class Goal
    {
        protected string Name { get; set; }
        protected MovingEntity Owner { get; set; }
        public GoalStatusType GoalStatus { get; set; }

        protected Goal(string name, MovingEntity entity)
        {
            Name = name;
            Owner = entity;
            GoalStatus = GoalStatusType.Inactive;
        }

        public abstract void Activate();
        public abstract void Process();
        public abstract void Deactivate();
        public abstract void Add(Goal goal);
        public abstract void RemoveFirst();
        public abstract void Display();
    }
}