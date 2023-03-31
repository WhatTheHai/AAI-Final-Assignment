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
        private const double staminaReductionValue = 0.3;

        public SeekTargetGoal(Enemy entity) : base(entity)
        {
            Name = "Seeking witch";
        }

        public override void Activate()
        {
            // todo:  what if target disapears or is null? other unit picked up? 
            Owner.SteeringBehaviour.Seek = true;
            SetActive();
        }


        public override void Process()
        {
            SetActiveIfInactive();
            Owner.Stamina -= staminaReductionValue;
            if (Owner.HasNoStamina() || Owner.HasLowHealth()) Deactivate();
        }

        public override void Deactivate()
        {
            Owner.SteeringBehaviour.Seek = false;
            SetComplete();
        }
    }
}