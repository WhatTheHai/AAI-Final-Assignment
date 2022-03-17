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
        private const bool F = false;
        public bool seek = F, flee = F, obstacleAvoidance = F;
        private MovingEntity ME { get; set; }

        public Vector2D Calculate() 
        {
            if (seek) {
                return new Vector2D(0, 0);
            }

            if (flee)
            {
                return new Vector2D(0, 0);
            }

            if (obstacleAvoidance)
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

        public Vector2D CalculateHide() 
        {
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();

            Vector2D desiredVelocity = targetPos.Sub(mePos).Normalize().Multiply(ME.MaxSpeed);

            return desiredVelocity.Sub(ME.Velocity.Clone());
        }
    }
}
