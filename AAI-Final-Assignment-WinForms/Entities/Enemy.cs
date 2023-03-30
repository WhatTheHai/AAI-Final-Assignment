using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
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
        public int Damage { get; set; }

        private int maxStamina;
        public double Stamina { get; set; }

        private const int lowHealthThreshold = 40;
        private const int FleeingThreshold = 400;

        public Enemy(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight, float mass,
            float maxSpeed, float maxForce, float radius, int stamina) : base(pos, world, scale, textureWidth,
            textureHeight,
            mass, maxSpeed,
            maxForce, radius)
        {
            // SteeringBehaviour.Arrive = false;
            SteeringBehaviour.Seek = false;
            SteeringBehaviour.Flee = false;
            SteeringBehaviour.ObstacleAvoidance = true;
            maxStamina = stamina;
            Stamina = stamina;

            MainGoal = new ThinkGoal(this);
            CurrentTarget = World.Witch;

            Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\sharkboy.png"),
                new Size(TextureWidth, TextureHeight));

            DetermineColor();
        }

        public void DetermineColor()
        {
            //Max mass is 100
            int redValue = (int)Math.Ceiling(255 * (Mass / 100));
            Color = Color.FromArgb(255, Math.Clamp(redValue, 0, 255), 0, 0);
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
            g.DrawEllipse(new Pen(Color, 3),
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

        public bool HasNoStamina()
        {
            return Stamina <= 0;
        }

        public bool HasMaxStamina()
        {
            return Stamina >= maxStamina;
        }

        public bool HasLowHealth()
        {
            return Health < lowHealthThreshold;
        }

        public bool IsToCloseToTarget(MovingEntity target)
        {
            return Pos.Distance(target.Pos) < FleeingThreshold;
        }
    }
}