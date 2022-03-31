using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Witch : MovingEntity
    {
        public Color WColor { get; set; }
        public Bitmap Texture { get; set; }

        public Witch(Vector2D pos, GameWorld w, float scale) : base(pos, w)
        {
            Velocity = new Vector2D(0, 0);
            Scale = scale;
            WColor = Color.Green;
            Texture = new Bitmap(Image.FromFile("..\\..\\..\\Sprites\\Wizard.png"), new Size(30, 30));
        }

        public override void Render(Graphics g)
        {
            double leftCorner = Pos.X - Scale;
            double rightCorner = Pos.Y - Scale;
            double size = Scale * 2;

            // Pen p = new Pen(WColor, 2);
            // g.DrawEllipse(p, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            // g.DrawLine(p, (int)Pos.X, (int)Pos.Y, (int)Pos.X + (int)(Velocity.X * 2), (int)Pos.Y + (int)(Velocity.Y * 2));
            g.DrawImage(Texture, (int)Pos.X - 15, (int)Pos.Y - 15);
        }
    }
}