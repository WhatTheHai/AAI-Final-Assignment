using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Entities;
using Timer = System.Timers.Timer;

namespace AAI_Final_Assignment_WinForms.Goals.Abstracts
{
    public abstract class AtomicGoal : Goal
    {
        protected Timer Timer;
        protected bool TimerComplete = false;

        protected AtomicGoal(string name, MovingEntity entity) : base(name, entity)
        {
        }


        public override string Display()
        {
            return "    " + Name;
        }
    }
}