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
    /// <summary>
    /// Select a item, seek the item.
    /// </summary>
    public class MoveToItemGoal : CompositeGoal
    {
        public MoveToItemGoal(Enemy entity) : base(entity)
        {
            Name = "MovingToItem";
        }

        public override void Activate()
        {
            SetActive();
            SubGoalsStack.Clear();
            SubGoalsStack.Push(new SeekItemGoal(Owner));
            SubGoalsStack.Push(new SelectItemGoal(Owner));
        }
    }
}