using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    internal class Obstacle : BaseGameEntity
    {
        public Obstacle(Vector2D pos, GameWorld world) : base(pos, world)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(Brushes.Black, new Rectangle((int)Pos.X, (int)Pos.Y, 50, 50));
        }
    }
}