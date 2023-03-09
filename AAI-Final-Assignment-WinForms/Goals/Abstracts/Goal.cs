using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Enums;

namespace AAI_Final_Assignment_WinForms.Goals.Abstracts
{
    /// <summary>
    /// Abstract goal class used for the composite pattern.
    /// </summary>
    public abstract class Goal
    {
        public string Name { get; set; }
        protected MovingEntity Owner { get; set; }
        public GoalStatusType GoalStatus { get; set; }

        protected Goal(MovingEntity entity)
        {
            Name = "";
            Owner = entity;
            GoalStatus = GoalStatusType.Inactive;
        }

        public abstract void Activate();
        public abstract void Process();
        public abstract void Deactivate();
        public abstract string Display();
    }
}