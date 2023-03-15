using System.Timers;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.Goals.Enums;
using Timer = System.Timers.Timer;

namespace AAI_Final_Assignment_WinForms.Goals
{
    /// <summary>
    ///  This goal activates the seeking behaviour of a moving entity.
    /// </summary>
    public class SeekItemGoal : AtomicGoal
    {
        public SeekItemGoal(MovingEntity entity) : base(entity)
        {
            Name = "Seeking item";
        }

        public override void Activate()
        {
            // what if target disapers or is null? other unit picked up? 
            Owner.SteeringBehaviour.Seek = true;
            GoalStatus = GoalStatusType.Active;
            // Timer = new Timer(5000);
            // Timer.Elapsed += TimerOnElapsed;
            // Timer.Enabled = true;
        }

        private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            //  todo: remove 
            TimerComplete = true;
        }

        public override void Process()
        {
            if (!IsActive()) Activate();
            if (!Owner.World.Items.Contains(Owner.CurrentTarget)) GoalStatus = GoalStatusType.Failed; // only works with items...
           
            
            // if (TimerComplete)
            // {
            //     GoalStatus = GoalStatusType.Completed;
            //     Deactivate();
            // }
        }

        public override void Deactivate()
        {
            Owner.SteeringBehaviour.Seek = false;
        }
    }
}