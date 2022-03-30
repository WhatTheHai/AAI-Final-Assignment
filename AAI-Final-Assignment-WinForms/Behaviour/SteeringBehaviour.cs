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
        public double DistanceAhead { get; set; }

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
            AheadVector2D = new Vector2D();
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


        public bool LineInCircle(Vector2D ahead, Vector2D aheadHalf, Circle circle)
        {
            DistanceAhead = circle.Center.Distance(ahead);
            double distanceAheadHalf = aheadHalf.Distance(circle.Center);
            double distanceME = ME.Pos.Distance(circle.Center);

            return DistanceAhead <= (circle.Radius / 2) || distanceAheadHalf <= (circle.Radius / 2) ||
                   distanceME <= (circle.Radius / 2);
            // return distanceME <= (circle.Radius / 2);
        }

        public Vector2D CalculateObstacleAvoidance()
        {
            // max
            double maxAhead = 25;
            double maxAvoidForce = 80;


            // fixed length for detection box 
            Vector2D ahead = ME.Pos.Clone();
            Vector2D heading = ME.Heading.Clone();
            heading.Multiply(maxAhead);
            ahead.Add(heading);
            AheadVector2D = ahead.Clone();
            Vector2D aheadHalf = ahead.Clone().Multiply(0.5);


            // nearest obstacle 
            Circle closestObstacle = null;
            Vector2D avoidanceForce = new Vector2D();

            // foreach circle check if there is a collision if so check if it is closer then other obstacle 
            foreach (Circle obstacle in ME.World.Obstacles)
            {
                bool collision = LineInCircle(ahead, aheadHalf, obstacle);

                if (collision && (closestObstacle == null ||
                                  ME.Pos.Distance(obstacle.Center) < ME.Pos.Distance(closestObstacle.Center)))
                {
                    closestObstacle = obstacle;
                }
            }

            if (closestObstacle != null)
            {
                avoidanceForce = ahead.Sub(closestObstacle.Center);
                avoidanceForce.Normalize();
                avoidanceForce.Multiply(maxAvoidForce);
                return avoidanceForce;
            }

            return new Vector2D();
        }
    }
}