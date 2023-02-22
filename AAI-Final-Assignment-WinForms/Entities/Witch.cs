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
        public Witch(Vector2D pos, GameWorld world, double scale, int textureWidth, int textureHeight, double mass,
            double maxSpeed, double maxForce, double radius
        ) : base(pos,
            world, scale, textureWidth, textureHeight, mass, maxSpeed, maxForce, radius)
        {
            Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\Wizard.png"),
                new Size(TextureWidth, TextureHeight));
        }

        public override void Render(Graphics g)
        {
            // double leftCorner = Pos.X - Scale;
            // double rightCorner = Pos.Y - Scale;
            // double size = Scale * 2;
            // Pen p = new Pen(Color.Orange, 2);
            // g.DrawEllipse(p, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            // g.DrawLine(p, (int)Pos.X, (int)Pos.Y, (int)Pos.X + (int)(Velocity.X * 2), (int)Pos.Y + (int)(Velocity.Y * 2));
            //g.DrawEllipse(new Pen(Color.Orange, 3), new Rectangle((int)Pos.X, (int)Pos.Y, TextureWidth, TextureHeight));

            g.DrawImage(Texture, (int)Pos.X - TextureWidth / 2, (int)Pos.Y - TextureHeight / 2);
        }
    }
}