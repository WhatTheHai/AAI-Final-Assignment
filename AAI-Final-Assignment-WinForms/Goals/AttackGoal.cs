using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.Goals.Enums;

namespace AAI_Final_Assignment_WinForms.Goals
{
    public class AttackGoal : CompositeGoal
    {
        public AttackGoal(MovingEntity entity) : base(entity)
        {
            Name = "Attacking";
        }

        public override void Activate()
        {
            GoalStatus = GoalStatusType.Active;
            SubGoalsStack.Clear();
            SubGoalsStack.Push(new SeekTargetGoal(Owner));
            SubGoalsStack.Push(new SelectWitchAsTargetGoal(Owner));
        }

        public override void Deactivate()
        {
            //
        }
    }
}