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
        private readonly Timer projectileTimer = new Timer(5000);
        public override void Update(double timeElapsed) { 
           Velocity = Heading.Clone().Multiply(4);
           Pos.Add(Velocity);
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(Brushes.Red, new Rectangle((int)(Pos.X - 1.5d), (int)(Pos.Y - 1.5d), 3, 3));
            g.DrawEllipse(new Pen(Color.Red, 3),
                new Rectangle((int)Pos.X - (int)Radius, (int)Pos.Y - (int)Radius, (int)Radius * 2, (int)Radius * 2));
        }

        public Projectile(Vector2D pos, GameWorld world, double scale, int textureWidth, int textureHeight, double mass,
            double maxSpeed, double maxForce, double radius) : base(pos, world, scale, textureWidth, textureHeight,
            mass, maxSpeed, maxForce, radius) {
            projectileTimer.Elapsed += OnProjectileTimerElapsed;
            projectileTimer.Enabled = true;
        }

        private void OnProjectileTimerElapsed(object sender, ElapsedEventArgs e) {
            World.MovingEntities.Remove(this);
        }
    }
}
