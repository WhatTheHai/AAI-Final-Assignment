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
    public class RestGoal : CompositeGoal
    {
        public RestGoal(Enemy entity) : base(entity)
        {
            Name = "Resting";
        }

        public override void Activate()
        {
            SubGoalsStack.Clear();
            SubGoalsStack.Push(new WanderGoal(Owner));
            SubGoalsStack.Push(new FleeGoal(Owner));
            SetActive();
        }
    }
}