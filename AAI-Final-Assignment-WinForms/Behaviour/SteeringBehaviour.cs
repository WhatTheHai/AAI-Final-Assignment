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
        public Vector2D totalForce { get; set; }
        Vector2D currentForce;
        public Vector2D AheadVector2D { get; set; }
        private MovingEntity ME { get; set; }

        public Vector2D Calculate()
        {
            totalForce = new Vector2D();
            if (ObstacleAvoidance)
            {
                currentForce = CalculateObstacleAvoidance();
                if (!AccumulateForce(totalForce, currentForce)) return totalForce;
            }

            if (Flee)
            {
                currentForce = CalculateFlee();
                if (!AccumulateForce(totalForce, currentForce)) return totalForce;
            }

            if (Seek)
            {
                currentForce = CalculateSeek();
                if (!AccumulateForce(totalForce, currentForce)) return totalForce;
            }

            if (Arrive)
            {
                currentForce = CalculateArrive();
                if (!AccumulateForce(totalForce, currentForce)) return totalForce;
            }


            return totalForce;
        }

        public SteeringBehaviour(MovingEntity me)
        {
            ME = me;
        }

        public bool AccumulateForce(Vector2D runningTotal, Vector2D forceToAdd)
        {
            double magnitudeSoFar = runningTotal.Length();
            double magnitudeRemaining = ME.MaxForce - magnitudeSoFar;
            if (magnitudeRemaining <= 0.0)
            {
                return false;
            }

            double magnitudeToAdd = forceToAdd.Length();

            if (magnitudeToAdd < magnitudeRemaining)
            {
                totalForce.Add(forceToAdd);
            }
            else
            {
                totalForce.Add(forceToAdd.Normalize().Multiply(magnitudeRemaining));
            }

            return true;
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

            if (dist > 0.0001)
            {
                double speed = dist / (deceleration * decelerationTweaker);
                speed = Math.Min(speed, ME.MaxSpeed);
                Vector2D desiredVelocity = toTarget.Multiply(speed / dist);

                return (desiredVelocity.Sub(ME.Velocity));
            }

            return new Vector2D(0, 0);
        }

        // public Vector2D CalculateObstacleAvoidance()
        // {
        //     // A. only consider obstacle in range of detection box 
        //     // loop to all objects to tag them if they are in range
        //     // B. transforms all tagged obstacle to vehicle local space
        //     // C. check obstacles for overlap of detection box 
        //     // D. got objects that ar e in detection box
        //     // 
        //
        //
        //     //  min length of detection box
        //     double minLength = 2;
        //
        //     // calculate length of detection box considering velocity 
        //     double detectionBoxLength = minLength + (ME.Velocity.Length() / ME.MaxSpeed) * minLength;
        //
        //     // tag obstacles in range of box 
        //     ME.TagObstacles(detectionBoxLength);
        //
        //     // keep track of closest obstacle 
        //     Obstacle closestIntersectingObstacle = null;
        //     double distanceToClosestObstacle = Double.MaxValue;
        //     Vector2D posOfClosestObstacle = null;
        //
        //     foreach (Obstacle obstacle in ME.World.Obstacles)
        //     {
        //         if (obstacle.IsTagged)
        //         {
        //             Vector2D localPos = Matrix2D.PointToLocalSpace(obstacle.Pos, ME.Heading, ME.Side, ME.Pos).Clone();
        //             if (localPos.X >= 0)
        //             {
        //                 double expandRadius = obstacle.BoundingRadius + ME.BoundingRadius;
        //                 if (Math.Abs(localPos.Y) < expandRadius)
        //                 {
        //                     double cX = localPos.X;
        //                     double cY = localPos.Y;
        //                     double SqrtPart = Math.Sqrt(expandRadius * expandRadius - cY * cY);
        //                     double ip = cX - SqrtPart;
        //
        //                     if (ip < distanceToClosestObstacle)
        //                     {
        //                         distanceToClosestObstacle = ip;
        //                         closestIntersectingObstacle = obstacle;
        //                         posOfClosestObstacle = localPos;
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //
        //     Vector2D steeringForce = new Vector2D();
        //
        //     if (closestIntersectingObstacle != null)
        //     {
        //         double multiplier = 1.0 + (detectionBoxLength - posOfClosestObstacle.X) / detectionBoxLength;
        //         steeringForce.Y = (closestIntersectingObstacle.BoundingRadius - posOfClosestObstacle.Y) * multiplier;
        //         double brakingWeight = 0.2;
        //
        //         steeringForce.X = (closestIntersectingObstacle.BoundingRadius - posOfClosestObstacle.X) * brakingWeight;
        //     }
        //
        //
        //     return null;
        // }

        public Vector2D CalculateObstacleAvoidance()
        {
            if (ME.Velocity.X == 0 && ME.Velocity.Y == 0) return new Vector2D();

            AheadVector2D = ME.Pos.Add(ME.Heading);
            Vector2D aheadVector2DHalf = AheadVector2D.Clone().Multiply(0.5);

            // Vector2D aheadVector2D = ME.Pos.Add(ME.Velocity.Normalize()).Multiply(maxAhead).Clone();
            //Vector2D aheadVector2DHalf = aheadVector2D.Multiply(0.5).Clone();

            Circle closestObstacle = null;
            Vector2D avoidance = new Vector2D();

            // todo fix obstacles/circle 
            foreach (Circle obstacle in ME.World.Obstacles)
            {
                bool collision = LineInCircle(AheadVector2D, aheadVector2DHalf, obstacle);

                // todo possible null 
                if (collision && (closestObstacle == null ||
                                  ME.Pos.Distance(obstacle.Center) < ME.Pos.Distance(closestObstacle.Center)))
                    closestObstacle = obstacle;
            }

            if (closestObstacle != null)
            {
                AheadVector2D = AheadVector2D.Clone().Sub(closestObstacle.Pos).Clone();
                return AheadVector2D;
            }
            else
            {
                return new Vector2D();
            }
        }

        public bool LineInCircle(Vector2D ahead, Vector2D aheadHalf, Circle circle)
        {
            double distanceAhead = ahead.Distance(circle.Center);
            double distanceAheadHalf = aheadHalf.Distance(circle.Center);
            double distanceME = ME.Pos.Distance(circle.Center);

            return distanceAhead <= (circle.Radius / 2) || distanceAheadHalf <= (circle.Radius / 2) ||
                   distanceME <= (circle.Radius / 2);
        }
    }
}