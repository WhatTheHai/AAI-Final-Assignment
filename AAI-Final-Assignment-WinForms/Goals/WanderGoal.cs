using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.Goals.Enums;
using Timer = System.Timers.Timer;

namespace AAI_Final_Assignment_WinForms.Goals
{
    /// <summary>
    ///  This goal activates the wander behaviour of a moving entity.
    /// </summary>
    public class WanderGoal : AtomicGoal
    {
        public WanderGoal(MovingEntity entity) : base(entity)
        {
            Name = "Wander";
        }

        public override void Activate()
        {
            Owner.SteeringBehaviour.Wander = true;
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
            Owner.SteeringBehaviour.Wander = false;
        }
    }
}