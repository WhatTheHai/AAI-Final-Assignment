using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals
{
    public class MoveToPosition : AtomicGoal
    {
        public MoveToPosition(MovingEntity entity) : base(entity)
        {
            Name = "Moving";
        }

        public override void Activate()
        {
            // target setten? 
            throw new NotImplementedException();
        }

        public override void Process()
        {
            throw new NotImplementedException();
        }

        public override void Deactivate()
        {
            // uitzetten 
            throw new NotImplementedException();
        }
    }
}