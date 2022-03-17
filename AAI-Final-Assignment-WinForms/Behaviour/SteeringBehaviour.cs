using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.Behaviour
{
    public class SteeringBehaviour 
    {
        public bool Seek, Flee, ObstacleAvoidance;
        private MovingEntity ME { get; set; }

        public Vector2D Calculate() 
        {
            if (Seek) {
                return new Vector2D(0, 0);
            }

            if (Flee)
            {
                return new Vector2D(0, 0);
            }

            if (ObstacleAvoidance)
            {
                return new Vector2D(0, 0);
            }

            return new Vector2D(0, 0);
        }

        protected SteeringBehaviour(MovingEntity me) {
            ME = me;
        }

        public Vector2D CalculateSeek() 
        {
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();

            Vector2D desiredVelocity = targetPos.Sub(mePos).Normalize().Multiply(ME.MaxSpeed);

            return desiredVelocity.Sub(ME.Velocity.Clone());
        }

        public Vector2D CalculateFlee() {
            double PanicDistanceSq = 10.0;
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();

            //If not in the panic distance, do not flee
            if (mePos.Distance(targetPos) > PanicDistanceSq) {
                return new Vector2D(0, 0);
            }

            Vector2D desiredVelocity = mePos.Sub(targetPos).Normalize().Multiply(ME.MaxSpeed);

            return desiredVelocity.Sub(ME.Velocity.Clone());
        }
    }
}
