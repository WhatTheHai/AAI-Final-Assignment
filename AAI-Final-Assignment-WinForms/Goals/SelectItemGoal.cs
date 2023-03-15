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
    public class SelectItemGoal : AtomicGoal
    {
        public SelectItemGoal(MovingEntity entity) : base(entity)
        {
            Name = "Select Item";
        }

        public override void Activate()
        {
            // een item selecteren 
            var itemCount = Owner.World.Items.Count;

            if (itemCount == 0) GoalStatus = GoalStatusType.Failed;
            else
            {
                GoalStatus = GoalStatusType.Active;

                Random random = new Random();
                Owner.CurrentTarget = Owner.World.Items[random.Next(itemCount)];
                GoalStatus = GoalStatusType.Completed;
            }
        }

        public override void Process()
        {
            if (!IsActive()) Activate();
          
        }

        public override void Deactivate()
        {
            // 
        }
    }
}