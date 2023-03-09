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
    public class SeekTargetGoal : AtomicGoal
    {
        public SeekTargetGoal(MovingEntity entity) : base(entity)
        {
            Name = "Seek";
        }

        public override void Activate()
        {
            Owner.SteeringBehaviour.Seek = true;
            GoalStatus = GoalStatusType.Active;
            Timer = new Timer(5000);
            Timer.Elapsed += TimerOnElapsed;
            Timer.Enabled = true;
        }

        private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            TimerComplete = true;
        }

        public override void Process()
        {
            if (GoalStatus == GoalStatusType.Inactive) Activate();
            if (TimerComplete)
            {
                GoalStatus = GoalStatusType.Completed;
                Deactivate();
            }
        }

        public override void Deactivate()
        {
            Owner.SteeringBehaviour.Seek = false;
        }
    }
}