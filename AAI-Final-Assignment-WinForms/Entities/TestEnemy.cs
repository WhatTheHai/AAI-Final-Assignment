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

        public TestEnemy(Vector2D pos, GameWorld world, double scale, int textureWidth, int textureHeight, double mass, double maxSpeed, double maxForce) : base(pos, world, scale, textureWidth, textureHeight, mass, maxSpeed, maxForce)
        {
            Color = Color.DarkOrchid;
            SteeringBehaviour.Arrive = false;
            SteeringBehaviour.Seek = true;
            SteeringBehaviour.ObstacleAvoidance = false;

            Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\sharkboy.png"),
                new Size(TextureWidth, TextureHeight));
        }

        // public TestEnemy(Vector2D pos, GameWorld world, float scale, float mass , float maxSpeed, float maxForce  ) : base(pos, world, scale, mass, maxSpeed, maxForce)
        // {
        //     Color = Color.DarkOrchid;
        //     SteeringBehaviour.Arrive = true;
        //     SteeringBehaviour.ObstacleAvoidance = true;
        // }

        public override void Render(Graphics g)
        {
            // double leftCorner = Pos.X - Scale;
            // double rightCorner = Pos.Y - Scale;
            // double size = Scale * 2;
            //
            // Pen p = new Pen(Color, 5);
            // g.DrawEllipse(p, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            // // g.DrawLine(p, (int)Pos.X, (int)Pos.Y, (int)Pos.X + (int)(Velocity.X * 2),
            // //     (int)Pos.Y + (int)(Velocity.Y * 2));
            //
            // g.DrawLine(p, (int)Pos.X, (int)Pos.Y, (int)SteeringBehaviour.TotalForce.X + (int)Pos.X,
            //     (int)SteeringBehaviour.TotalForce.Y + (int)Pos.Y);
            // //
            // length of detection box 
            g.DrawLine(new Pen(Color.Orange, 5), (int)Pos.X, (int)Pos.Y,
                (int)SteeringBehaviour.AheadVector2D.X,
                (int)SteeringBehaviour.AheadVector2D.Y);

            g.DrawImage(Texture, (int)Pos.X - TextureWidth / 2, (int)Pos.Y - TextureHeight / 2);

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            float x = (float)10;
            float y = (float)10;
            StringFormat drawFormat = new StringFormat();
            //drawFormat.FormatFlags = StringFormatFlags.NoWrap;

            g.DrawString($"Velocity: {Velocity}", drawFont, drawBrush, x, y, drawFormat);
            g.DrawString($"Heading: {Heading}", drawFont, drawBrush, x, y + 20, drawFormat);
            g.DrawString($"Forces: {SteeringBehaviour.TotalForce}", drawFont, drawBrush, x, y + 40, drawFormat);
            // g.DrawString($"Velocity: {Velocity}", drawFont, drawBrush, x, y, drawFormat);
            // g.DrawString($"Velocity: {Velocity}", drawFont, drawBrush, x, y, drawFormat);
            // g.DrawString($"Velocity: {Velocity}", drawFont, drawBrush, x, y, drawFormat);
            // g.DrawString($"Velocity: {Velocity}", drawFont, drawBrush, x, y, drawFormat);
        }

       
    }
}