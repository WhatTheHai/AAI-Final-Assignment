using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Circle : BaseGameEntity
    {
        public int Radius { get; set; }
        public Vector2D Center { get; set; }

        public Circle(Vector2D pos, GameWorld world) : base(pos, world)
        {
            Radius = 40;
            Center = new Vector2D(Pos.X + (Radius / 2), Pos.Y + (Radius / 2));
        }

        public override void Render(Graphics g)
        {
            // g.FillEllipse(Brushes.Black, new Rectangle((int)Pos.X, (int)Pos.Y, Radius, Radius));
            g.DrawEllipse(new Pen(Color.Black, 3), new Rectangle((int)Pos.X, (int)Pos.Y, Radius, Radius));
            g.FillRectangle(Brushes.Black, new Rectangle((int)Center.X, (int)Center.Y, 1, 1));
        }
    }
}