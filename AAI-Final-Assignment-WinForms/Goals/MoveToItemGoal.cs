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
    public class MoveToItemGoal : CompositeGoal
    {
        public MoveToItemGoal(MovingEntity entity) : base(entity)
        {
            Name = "MovingToItem";
        }

        public override void Activate()
        {
            GoalStatus = GoalStatusType.Active;
            SubGoalsStack.Clear();
            // pick up item? 
            SubGoalsStack.Push(new SeekTargetGoal(Owner));
            SubGoalsStack.Push(new SelectItemGoal(Owner)); // first 
        }

        public override void Process()
        {
            // scuffed
            if (!IsActive()) Activate();
            var currentGoal = SubGoalsStack.Peek();

            if (currentGoal == null)
            {
                GoalStatus = GoalStatusType.Completed;
            }
            else
            {
                if (currentGoal.GoalStatus == GoalStatusType.Completed ||
                    currentGoal.GoalStatus == GoalStatusType.Failed)
                {
                    SubGoalsStack.Pop().Deactivate();
                }

                if (SubGoalsStack.Count > 0)
                {
                    currentGoal = SubGoalsStack.Peek();
                    if (currentGoal != null)
                        currentGoal.Process();
                }
                else
                {
                    GoalStatus = GoalStatusType.Completed;
                }
            }
        }

        public override void Deactivate()
        {
            // uitzetten 
        }
    }
}