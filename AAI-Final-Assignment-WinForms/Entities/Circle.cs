using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Circle : StaticEntity
    {
        public int Diameter { get; set; }
        public Vector2D Center { get; set; }

        public Circle(Vector2D pos, GameWorld world, float scale, int diameter, int textureWidth, int textureHeight,
            double radius) :
            base(pos, world, scale, textureWidth, textureHeight, radius)
        {
            Diameter = diameter * (int)scale;
            Center = new Vector2D(Pos.X + (Diameter / 2), Pos.Y + (Diameter / 2));
            Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\CAULDRON FILL.png"),
                new Size(TextureWidth, TextureHeight));
        }


        public override void Render(Graphics g)
        {
            // g.FillEllipse(Brushes.Black, new Rectangle((int)Pos.X, (int)Pos.Y, Diameter, Diameter));
            g.DrawEllipse(new Pen(Color.Black, 3), new Rectangle((int)Pos.X, (int)Pos.Y, Diameter, Diameter));
            // g.FillRectangle(Brushes.Black, new Rectangle((int)Center.X, (int)Center.Y, 1, 1));

            g.DrawImage(Texture, (int)Center.X - TextureWidth / 2, (int)Center.Y - TextureHeight / 2);
        }
    }
}