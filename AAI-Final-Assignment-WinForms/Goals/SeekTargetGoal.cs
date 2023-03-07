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
    public class SeekTargetGoal : AtomicGoal
    {
        public SeekTargetGoal(string name, MovingEntity entity) : base(name, entity)
        {
        }

        public override void Activate()
        {
            Owner.SteeringBehaviour.Seek = true;
            GoalStatus = GoalStatusType.Active;
        }

        public override void Process()
        {
            if (GoalStatus == GoalStatusType.Inactive) Activate();
        }

        public override void Deactivate()
        {
            Owner.SteeringBehaviour.Seek = false;
        }

        public override void Display()
        {
            throw new NotImplementedException();
        }
    }
}