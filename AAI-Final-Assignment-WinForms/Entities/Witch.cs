using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AAI_Final_Assignment_WinForms.Graph;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;
using Timer = System.Timers.Timer;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Witch : MovingEntity
    {
        private readonly Timer shootTimer = new Timer(1000);
        private Vector2D desiredVertex = new Vector2D();

        public Witch(Vector2D pos, GameWorld world, double scale, int textureWidth, int textureHeight, double mass,
            double maxSpeed, double maxForce, double radius
        ) : base(pos,
            world, scale, textureWidth, textureHeight, mass, maxSpeed, maxForce, radius)
        {
            Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\Wizard.png"),
                new Size(TextureWidth, TextureHeight));
            MaxHealth = Health = 500;

            shootTimer.Elapsed += OnShootTimerElapsed;
            shootTimer.AutoReset = true;
            shootTimer.Enabled = true;
        }

        public void SetDestination(Vector2D destinationPos)
        {
            desiredVertex = World.GameGraph.ClosestVertex(destinationPos);
            World.GameGraph.MovePath = World.GameGraph.AStar(this.Pos, desiredVertex);
        }

        public void CheckWithinRange(List<BaseGameEntity> entities)
        {
            foreach (BaseGameEntity entity in entities)
            {
                if (entity is TestEnemy enemyEntity)
                {
                    if (enemyEntity.Pos.Distance(Pos) < entity.Radius + Radius) {
                        var test = enemyEntity.Pos.Distance(Pos);
                        // Witch takes damage
                        Health -= 1;
                    }
                }
                else if (entity is ItemSpawn item && item.Pos.Clone().Sub(Pos).Length() < item.Radius + Radius)
                {
                    // Witch heals
                    World.Items.Remove(item);
                    if (Health + 10 >= MaxHealth) {
                        Health = MaxHealth;
                    }
                    else {
                        Health += 10;
                    }
                }
            }
        }

        public void ShootNearbyEnemy() {
            TestEnemy? nearestEnemy = GetNearestEnemy(World.MovingEntities);
            if (nearestEnemy != null && nearestEnemy.Pos.Clone().Distance(Pos) < 300) {
                var heading = nearestEnemy.Pos.Clone().Sub(Pos).Normalize();
                World.SpawnProjectile(Pos.Clone(), heading);
            }
        }

        public TestEnemy? GetNearestEnemy(List<BaseGameEntity> entities)
        {
            TestEnemy nearestEnemy = null;
            double nearestDistance = double.MaxValue;
    
            foreach (BaseGameEntity entity in entities.ToList())
            {
                if (entity is TestEnemy movingEntity)
                {
                    double distance = movingEntity.Pos.Distance(Pos);
                    if (distance < nearestDistance)
                    {
                        nearestEnemy = movingEntity;
                        nearestDistance = distance;
                    }
                }
            }

            return nearestEnemy;
        }

        public override void Render(Graphics g)
        {
            // g.DrawImage(Texture, (int)Pos.X - TextureWidth / 2, (int)Pos.Y - TextureHeight / 2);
            g.FillEllipse(Brushes.Blue, new Rectangle((int)Pos.X, (int)Pos.Y, 3, 3));
            g.DrawEllipse(new Pen(Color.Blue, 3),
                new Rectangle((int)Pos.X - (int)Radius, (int)Pos.Y - (int)Radius, (int)Radius * 2, (int)Radius * 2));

            RenderHp(g);
        }

        public override void Update(double timeElapsed) {
            if (World.GameGraph.MovePath != null && World.GameGraph.MovePath.Count > 0)
            {
                // Get the first vertex of the move path
                Vector2D firstVector = World.GameGraph.MovePath.First().Clone();
                // If the witch is close enough to the first vertex, remove it from the move path
                if (firstVector.Clone().Sub(Pos).Length() < 2)
                {
                    World.GameGraph.MovePath.RemoveAt(0);
                    return;
                }

                // Calculate the direction vector from the witch's current position to the first vertex
                Vector2D direction = firstVector.Sub(Pos);

                // Check if the direction vector has a length of zero
                // Edge case check
                if (direction.Length() == 0)
                {
                    Velocity = new Vector2D();
                }
                else
                {
                    // Otherwise, normalize the direction vector and calculate the velocity vector
                    direction.Normalize();
                    Velocity = direction.Multiply(4);
                }

                // Update the position of the witch
                Pos.Add(Velocity.Multiply(timeElapsed));
            }
        }

        private void OnShootTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ShootNearbyEnemy();
        }
    }
}