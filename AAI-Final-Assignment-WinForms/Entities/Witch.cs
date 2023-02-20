using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Graph;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Witch : MovingEntity
    {
        private Vector2D currentVertex;
        private Vector2D desiredVertex = new Vector2D();
        public Witch(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight, float mass,
            float maxSpeed, float maxForce
        ) : base(pos,
            world, scale, textureWidth, textureHeight, mass, maxSpeed, maxForce)
        {
            Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\Wizard.png"),
                new Size(TextureWidth, TextureHeight));
        }

        public void setDestination(Vector2D destinationPos)
        {
            desiredVertex = World.GameGraph.ClosestVertex(destinationPos);
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
            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            float x = (float)Pos.X - 20;
            float y = (float)Pos.Y - 80;
            StringFormat drawFormat = new StringFormat();
            g.DrawImage(Texture, (int)Pos.X - TextureWidth / 2, (int)Pos.Y - TextureHeight / 2);
            g.DrawString($"Vertex: {currentVertex}", drawFont, drawBrush, x+100, y+200, drawFormat);
            g.DrawString($"Vertex: {desiredVertex}", drawFont, drawBrush, x+100, y+250, drawFormat);
            
        }
        public override void Update(double timeElapsed)
        {
/*            if (!currentVertex.Equals(desiredVertex))
            {
                currentVertex = World.GameGraph.ClosestVertex(Pos);
            }*/
        }

    }
}