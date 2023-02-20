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
        public bool Seek, Flee, Arrive, ObstacleAvoidance, Pursuit;
        public Vector2D TotalForce { get; set; }
        public Vector2D CurrentForce;
        public Vector2D AheadVector2D { get; set; }
        private MovingEntity ME { get; set; }
        public double DistanceAhead { get; set; }

        public Vector2D CurrentObstacleAvoidance;
        public Vector2D CurrentDesiredForceSeek = new();

        public Vector2D CurrentArrive;

        public Vector2D CurrentDesiredForceObstacle = new();
        public Vector2D CurrentDesiredForceArrive = new();
        public int IdClosestObject = -1;
        public int LastSeen = -1;

        public double Dist;

        public Vector2D Calculate()
        {
            TotalForce = new Vector2D();
            if (ObstacleAvoidance)
            {
                CurrentForce = CalculateObstacleAvoidance();
                if (!AccumulateForce(TotalForce, CurrentForce)) return TotalForce;
            }

            if (Flee)
            {
                CurrentForce = CalculateFlee();
                //if (!AccumulateForce(TotalForce, CurrentForce)) return TotalForce;
            }

            if (Seek)
            {
                CurrentForce = CalculateSeek();

                if (!AccumulateForce(TotalForce, CurrentForce)) return TotalForce;
            }

            if (Pursuit)
            {
                CurrentForce = CalculateArrive();
                if (!AccumulateForce(TotalForce, CurrentForce)) return TotalForce;
            }

            if (Arrive)
            {
                CurrentForce = CalculateArrive();
                if (!AccumulateForce(TotalForce, CurrentForce)) return TotalForce;
            }


            return TotalForce;
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
                TotalForce.Add(forceToAdd);
            }
            else
            {
                TotalForce.Add(forceToAdd.Normalize().Multiply(magnitudeRemaining));
            }

            return true;
        }

        public Vector2D CalculateSeek()
        {
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();

            //Vector2D desiredVelocity = targetPos.Sub(mePos).Normalize().Multiply(ME.MaxSpeed);
            Vector2D desiredVelocity = targetPos.Sub(mePos);
            desiredVelocity.Normalize();
            desiredVelocity.Multiply(ME.MaxSpeed);
            desiredVelocity.Sub(ME.Velocity);

            CurrentDesiredForceSeek = desiredVelocity.Clone();


            return desiredVelocity;
            // return desiredVelocity;
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

        // public Vector2D CalculatePursuit()
        // {
        //     Vector2D ToEvader = ME.World.Witch.Pos.Clone();
        //
        //     double relativeHeading = ME.Heading.Clone().Dot()
        // }

        // public Vector2D CalculateArriveOLD()
        // {
        //     const double decelerationTweaker = 2;
        //     //1 = fast, 2 = normal, 3 = slow
        //     const double deceleration = 46;
        //
        //     Vector2D mePos = ME.Pos.Clone();
        //     Vector2D targetPos = ME.World.Witch.Pos.Clone();
        //     Vector2D toTarget = targetPos.Sub(mePos);
        //
        //      Dist = toTarget.Length();
        //
        //     if (Dist > 0.0001)
        //     {
        //         double speed = Dist / (deceleration * decelerationTweaker);
        //         speed = Math.Min(speed, ME.MaxSpeed);
        //         Vector2D desiredVelocity = toTarget.Multiply(speed / Dist);
        //         desiredVelocity.Sub(ME.Velocity);
        //         CurrentDesiredForceArrive = desiredVelocity;
        //         return desiredVelocity;
        //     }
        //
        //     CurrentDesiredForceArrive = new Vector2D();
        //
        //     return new Vector2D();
        // }

        public Vector2D CalculateArrive()
        {
            const double deceleration = 175;


            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();
            Vector2D toTarget = targetPos.Sub(mePos);

            Dist = toTarget.Length();

            if (Dist > 0.0001)
            {
                double speed = Dist / deceleration;
                speed = Math.Min(speed, ME.MaxSpeed);

                Vector2D desiredVelocity = toTarget.Multiply(speed / Dist);
                desiredVelocity.Sub(ME.Velocity);
                CurrentDesiredForceArrive = desiredVelocity;
                return desiredVelocity;
            }

            CurrentDesiredForceArrive = new Vector2D();

            return new Vector2D();
        }


        public bool LineInCircle(Vector2D ahead, Vector2D aheadHalf, Circle circle)
        {
            DistanceAhead = circle.Center.Distance(ahead);
            double distanceAheadHalf = aheadHalf.Distance(circle.Center);
            double distanceME = ME.Pos.Distance(circle.Center);

            return DistanceAhead <= (circle.Diameter / 2) || distanceAheadHalf <= (circle.Diameter / 2) ||
                   distanceME <= (circle.Diameter / 2);
        }

        public Vector2D CalculateObstacleAvoidance()
        {
            // max
            double maxAhead = 0.5;
            double maxAvoidForce = 50;


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
            foreach (Circle obstacle in ME.World.StaticEntities)
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

                CurrentDesiredForceObstacle = avoidanceForce;
                IdClosestObject = closestObstacle.Id;
                LastSeen = closestObstacle.Id;
                return avoidanceForce;
            }

            IdClosestObject = -1;
            CurrentDesiredForceObstacle = new Vector2D();
            return new Vector2D();
        }
    }
}