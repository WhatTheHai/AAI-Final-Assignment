using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Circle : ObstacleEntity
    {
        public int Radius { get; set; }

        public Circle(Vector2D pos, GameWorld world) : base(pos, world)
        {
            Radius = 50;
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(Brushes.Black, new Rectangle((int)Pos.X, (int)Pos.Y, Radius, Radius));
        }
    }
}