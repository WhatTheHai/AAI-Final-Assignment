using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;
using Timer = System.Timers.Timer;

namespace AAI_Final_Assignment_WinForms.Entities
{

    public class Projectile : MovingEntity {
        private readonly Timer projectileTimer = new Timer(2500);
        
        public Projectile(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight, float mass,
            float maxSpeed, float maxForce, float radius, Vector2D heading) : base(pos, world, scale, textureWidth, textureHeight,
            mass, maxSpeed, maxForce, radius) {
            projectileTimer.Elapsed += OnProjectileTimerElapsed;
            projectileTimer.Enabled = true;
            Heading = heading;
            Velocity = Heading.Clone().Multiply(5);
        }

        public override void Update(float timeElapsed) {
            Pos.Add(Velocity);
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(Brushes.Red, new Rectangle((int)(Pos.X - 1.5d), (int)(Pos.Y - 1.5d), 3, 3));
            g.DrawEllipse(new Pen(Color.Red, 3),
                new Rectangle((int)Pos.X - (int)Radius, (int)Pos.Y - (int)Radius, (int)Radius * 2, (int)Radius * 2));
        }

        private void OnProjectileTimerElapsed(object sender, ElapsedEventArgs e) {
            World.MovingEntities.Remove(this);
        }
    }
}
