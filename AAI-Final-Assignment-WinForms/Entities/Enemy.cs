using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Goals;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Enemy : MovingEntity
    {
        public Color Color { get; set; }
        public Goal MainGoal { get; set; }
        public int Damage { get; set; } = 10;

        public Enemy(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight, float mass,
            float maxSpeed, float maxForce, float radius) : base(pos, world, scale, textureWidth,
            textureHeight,
            mass, maxSpeed,
            maxForce, radius)
        {
            Color = Color.DarkOrchid;
            SteeringBehaviour.Arrive = false;
            SteeringBehaviour.Seek = false;
            SteeringBehaviour.ObstacleAvoidance = true;

            MainGoal = new ThinkGoal(this);
            CurrentTarget = World.Witch;

            Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\sharkboy.png"),
                new Size(TextureWidth, TextureHeight));
        }

        public override void Update(float timeElapsed)
        {
            if (Health <= 0)
            {
                World.MovingEntities.Remove(this);
            }

            MainGoal.Process();
            base.Update(timeElapsed);
        }

        public override void Render(Graphics g)
        {
            g.DrawEllipse(new Pen(Color.Red, 3),
                new Rectangle((int)Pos.X - (int)Radius, (int)Pos.Y - (int)Radius, (int)Radius * 2, (int)Radius * 2));

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Yellow);
            float x = (float)Pos.X + 10;
            float y = (float)Pos.Y + 10;
            StringFormat drawFormat = new StringFormat();


            if (World.ShowGoals) g.DrawString(MainGoal.Display(), drawFont, drawBrush, x, y, drawFormat);
            RenderHp(g);
            if (World.ShowForces) RenderForceArrows(g);
        }
    }
}