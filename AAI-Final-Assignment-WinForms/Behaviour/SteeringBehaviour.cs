using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.Behaviour
{
    public class SteeringBehaviour
    {
        public bool Seek, Flee, Arrive, ObstacleAvoidance;
        private MovingEntity ME { get; set; }

        public Vector2D Calculate()
        {
            if (Seek)
            {
                return CalculateSeek();
            }

            if (Flee)
            {
                return CalculateFlee();
            }

            if (Arrive)
            {
                return CalculateArrive();
            }

            if (ObstacleAvoidance)
            {
                return new Vector2D(0, 0);
            }

            return new Vector2D(0, 0);
        }

        public SteeringBehaviour(MovingEntity me)
        {
            ME = me;
        }

        public Vector2D CalculateSeek()
        {
            double MaxCloseDistance = 10.0;
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();
            if (mePos.Distance(targetPos) < MaxCloseDistance)
            {
                return new Vector2D(0, 0);
            }

            Vector2D desiredVelocity = targetPos.Sub(mePos).Normalize().Multiply(ME.MaxSpeed);

            return desiredVelocity.Sub(ME.Velocity.Clone());
        }

        public Vector2D CalculateFlee()
        {
            double PanicDistanceSq = 100.0;
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();

            //If not in the panic distance, do not flee
            if (mePos.Distance(targetPos) > PanicDistanceSq)
            {
                return new Vector2D(0, 0);
            }

            Vector2D desiredVelocity = mePos.Sub(targetPos).Normalize().Multiply(ME.MaxSpeed);

            return desiredVelocity.Sub(ME.Velocity.Clone());
        }

        public Vector2D CalculateArrive()
        {
            const double decelerationTweaker = 0.8;
            //1 = fast, 2 = normal, 3 = slow
            const double deceleration = 1;
            
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();
            Vector2D toTarget = targetPos.Sub(mePos);

            double dist = toTarget.Length();

            if (dist > 0.0001) {
                double speed = dist / (deceleration * decelerationTweaker);
                speed = Math.Min(speed, ME.MaxSpeed);
                Vector2D desiredVelocity = toTarget.Multiply(speed / dist);

                return (desiredVelocity.Sub(ME.Velocity));
            }

            return new Vector2D(0, 0);
        }

        public Vector2D CalculateObstacleAvoidance()
        {
            // A. only consider obstacle in range of detection box 
            // loop to all objects to tag them if they are in range
            // B. transforms all tagged obstacle to vehicle local space
            // C. check obstacles for overlap of detection box 
            // D. got objects that ar e in detection box
            // 


            //  min length of detection box
            double minLength = 2;

            // calculate length of detection box considering velocity 
            double detectionBoxLength = minLength + (ME.Velocity.Length() / ME.MaxSpeed) * minLength;

            // tag obstacles in range of box 
            ME.TagObstacles(detectionBoxLength);

            // keep track of closest obstacle 
            Obstacle closestIntersectingObstacle = null;
            double distanceToClosestObstacle = Double.MaxValue;
            Vector2D posOfClosestObstacle = null;

            foreach (Obstacle obstacle in ME.World.Obstacles)
            {
                if (obstacle.IsTagged)
                {
                    Vector2D localPos = Matrix2D.PointToLocalSpace(obstacle.Pos, ME.Heading, ME.Side, ME.Pos).Clone();
                    if (localPos.X >= 0)
                    {
                        double expandRadius = obstacle.BoundingRadius + ME.BoundingRadius;
                        if (Math.Abs(localPos.Y) < expandRadius)
                        {
                            double cX = localPos.X;
                            double cY = localPos.Y;
                            double SqrtPart = Math.Sqrt(expandRadius * expandRadius - cY * cY);
                            double ip = cX - SqrtPart;

                            if (ip < distanceToClosestObstacle)
                            {
                                distanceToClosestObstacle = ip;
                                closestIntersectingObstacle = obstacle;
                                posOfClosestObstacle = localPos;
                            }
                        }
                    }
                }
            }

            Vector2D steeringForce = new Vector2D();

            if (closestIntersectingObstacle != null)
            {
                double multiplier = 1.0 + (detectionBoxLength - posOfClosestObstacle.X) / detectionBoxLength;
                steeringForce.Y = (closestIntersectingObstacle.BoundingRadius - posOfClosestObstacle.Y) * multiplier;
                double brakingWeight = 0.2;

                steeringForce.X = (closestIntersectingObstacle.BoundingRadius - posOfClosestObstacle.X) * brakingWeight;
            }


            return null;
        }
    }
}