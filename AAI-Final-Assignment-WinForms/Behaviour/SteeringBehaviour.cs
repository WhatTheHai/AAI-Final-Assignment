using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.Behaviour
{
    public abstract class SteeringBehaviour
    {
        private MovingEntity ME { get; set; }
        public abstract Vector2D Calculate();

        protected SteeringBehaviour(MovingEntity me) {
            ME = me;
        }
    }
}
