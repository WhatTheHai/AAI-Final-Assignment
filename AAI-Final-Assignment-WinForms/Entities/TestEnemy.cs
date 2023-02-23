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

        public TestEnemy(Vector2D pos, GameWorld world, double scale, int textureWidth, int textureHeight, double mass,
            double maxSpeed, double maxForce, double radius) : base(pos, world, scale, textureWidth, textureHeight,
            mass, maxSpeed,
            maxForce, radius)
        {
            Color = Color.DarkOrchid;
            SteeringBehaviour.Arrive = false;
            SteeringBehaviour.Seek = true;
            SteeringBehaviour.ObstacleAvoidance = true;

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
            //   g.DrawImage(Texture, (int)Pos.X - TextureWidth / 2, (int)Pos.Y - TextureHeight / 2);

            // g.DrawLine(new Pen(Color.Orange, 5), (int)Pos.X, (int)Pos.Y,
            //     (int)SteeringBehaviour.AheadVector2D.X,
            //     (int)SteeringBehaviour.AheadVector2D.Y);
            g.FillEllipse(Brushes.Blue, new Rectangle((int)Pos.X, (int)Pos.Y, 3, 3));
            g.DrawEllipse(new Pen(Color.Red, 3),
                new Rectangle((int)Pos.X - (int)Radius / 2, (int)Pos.Y - (int)Radius / 2, (int)Radius, (int)Radius));

            base.Render(g);
        }
    }
}