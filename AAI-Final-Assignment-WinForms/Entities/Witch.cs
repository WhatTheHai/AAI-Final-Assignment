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
        private Vector2D desiredVertex = new Vector2D();

        public Witch(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight, float mass,
            float maxSpeed, float maxForce, float radius
        ) : base(pos,
            world, scale, textureWidth, textureHeight, mass, maxSpeed, maxForce, radius)
        {
            Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\Wizard.png"),
                new Size(TextureWidth, TextureHeight));
        }

        public void SetDestination(Vector2D destinationPos)
        {
            desiredVertex = World.GameGraph.ClosestVertex(destinationPos);
            World.GameGraph.MovePath = World.GameGraph.AStar(this.Pos, desiredVertex);
        }

        public override void Render(Graphics g)
        {
            // g.DrawImage(Texture, (int)Pos.X - TextureWidth / 2, (int)Pos.Y - TextureHeight / 2);
            g.FillEllipse(Brushes.Blue, new Rectangle((int)Pos.X, (int)Pos.Y, 3, 3));
            g.DrawEllipse(new Pen(Color.Blue, 3),
                new Rectangle((int)Pos.X - (int)Radius / 2, (int)Pos.Y - (int)Radius / 2, (int)Radius, (int)Radius));
        }

        public override void Update(float timeElapsed)
        {
            if (World.GameGraph.MovePath != null && World.GameGraph.MovePath.Count > 0)
            {
                // Get the first vertex of the move path
                Vector2D firstVector = World.GameGraph.MovePath.First().Clone();
                // If the witch is close enough to the first vertex, remove it from the move path
                if (firstVector.Clone().Sub(Pos).Length() < 1.5)
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
                    Velocity = direction.Multiply(3);
                }

                // Update the position of the witch
                Pos.Add(Velocity.Multiply(timeElapsed));
            }
        }
    }
}