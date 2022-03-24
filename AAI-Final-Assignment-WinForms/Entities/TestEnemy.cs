using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class TestEnemy : MovingEntity
    {
        public Color Color { get; set; }

        public TestEnemy(Vector2D pos, GameWorld world, float scale) : base(pos, world)
        {
            Velocity = new Vector2D(0, 0);
            Scale = scale;
            Color = Color.DarkOrchid;
            SteeringBehaviour.Seek = true;
            SteeringBehaviour.ObstacleAvoidance = true;
        }

        public override void Render(Graphics g)
        {
            double leftCorner = Pos.X - Scale;
            double rightCorner = Pos.Y - Scale;
            double size = Scale * 2;

            Pen p = new Pen(Color, 2);
            g.DrawEllipse(p, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            g.DrawLine(p, (int)Pos.X, (int)Pos.Y, (int)Pos.X + (int)(Velocity.X * 2),
                (int)Pos.Y + (int)(Velocity.Y * 2));
        }
    }
}