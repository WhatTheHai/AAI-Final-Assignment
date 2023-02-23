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
        // todo: get setters? initialize total/currentforce
        // All steering behaviour that can be enabled
        public bool Seek, Flee, Arrive, ObstacleAvoidance;

        // Total force on the object
        public Vector2D TotalForce { get; set; }

        // Current force of the object
        public Vector2D CurrentForce;

        // The entity that the steering behaviour is applied to 
        private MovingEntity ME { get; set; }


        // All vectors and booleans of the forces used for debugging and printing on screen
        public Vector2D CurrentObstacleAvoidance = new();
        public Vector2D CurrentSeek = new();
        public Vector2D CurrentArrive = new();

        public bool IsCollision { get; set; }

        /// <summary>
        /// Calculates all different forces together till max force of entity is reached
        /// </summary>
        /// <returns>The total force applied to the entity</returns>
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
        }

        /// <summary>
        /// Calculates if the maximum force is reached of the entity. 
        /// </summary>
        /// <param name="runningTotal"></param>
        /// <param name="forceToAdd"></param>
        /// <returns>A boolean that is false if max force is reached </returns>
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

        /// <summary>
        /// Calculates the desired force for seeking
        /// </summary>
        /// <returns>The seeking force</returns>
        public Vector2D CalculateSeek()
        {
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();

            //Vector2D desiredVelocity = targetPos.Sub(mePos).Normalize().Multiply(ME.MaxSpeed);
            Vector2D desiredVelocity = targetPos.Sub(mePos);
            desiredVelocity.Normalize();
            desiredVelocity.Multiply(ME.MaxSpeed);
            desiredVelocity.Sub(ME.Velocity);

            CurrentSeek = desiredVelocity.Clone();

            return desiredVelocity;
        }

        /// <summary>
        /// Calculates the desired force for fleeing
        /// </summary>
        /// <returns>The fleeing force</returns>
        public Vector2D CalculateFlee()
        {
            // todo: rework... 
            double panicDistanceSq = 100.0;
            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();

            //If not in the panic distance, do not flee
            if (mePos.Distance(targetPos) > panicDistanceSq)
            {
                return new Vector2D(0, 0);
            }

            Vector2D desiredVelocity = mePos.Sub(targetPos).Normalize().Multiply(ME.MaxSpeed);

            return desiredVelocity.Sub(ME.Velocity.Clone());
        }

        /// <summary>
        /// Calculates the desired force for arriving at a target
        /// </summary>
        /// <returns>The arriving force</returns>
        public Vector2D CalculateArrive()
        {
            const double deceleration = 175;


            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();
            Vector2D toTarget = targetPos.Sub(mePos);

            double dist = toTarget.Length();

            if (dist > 0.0001)
            {
                double speed = dist / deceleration;
                speed = Math.Min(speed, ME.MaxSpeed);

                Vector2D desiredVelocity = toTarget.Multiply(speed / dist);
                desiredVelocity.Sub(ME.Velocity);
                CurrentArrive = desiredVelocity;
                return desiredVelocity;
            }

            CurrentArrive = new Vector2D();

            return new Vector2D();
        }

        /// <summary>
        /// Calculates the desired force for avoiding a obstacle
        /// </summary>
        /// <returns>The avoidance force</returns>
        public Vector2D CalculateObstacleAvoidance()
        {
            // Range for looking ahead of entity
            double maxSeeAhead = 50;
            // Calculate dynamic length
            double length = ME.Velocity.Clone().Length() / ME.MaxSpeed;

            //todo: max avoidance force, maybe use local
            Vector2D avoidanceForce = new Vector2D();

            // Max range vector
            Vector2D aheadVector = ME.Velocity.Clone().Normalize().Multiply(length).Multiply(maxSeeAhead)
                .Add(ME.Pos.Clone());
            // Half of the max range vector
            Vector2D aheadVectorHalf = ME.Velocity.Clone().Normalize().Multiply(length).Multiply(maxSeeAhead)
                .Multiply(0.5).Add(ME.Pos.Clone());

            // Look for closest obstacle
            StaticEntity? closestObstacle = FindClosestObstacle(aheadVector, aheadVectorHalf);

            if (closestObstacle == null) return avoidanceForce;

            // calculate force
            avoidanceForce = aheadVector.Clone().Sub(closestObstacle.Pos.Clone());
            avoidanceForce.Normalize();
            avoidanceForce.Multiply(ME.MaxForce);

            CurrentObstacleAvoidance = avoidanceForce.Clone();

            return avoidanceForce;
        }

        /// <summary>
        ///  Check if the ahead/half ahead/character are in the radius of the obstacle
        /// </summary>
        /// <param name="ahead"></param>
        /// <param name="aheadHalf"></param>
        /// <param name="obstacle"></param>
        /// <returns>boolean</returns>
        private bool LineIntersectsCircle(Vector2D ahead, Vector2D aheadHalf, StaticEntity obstacle)
        {
            return obstacle.Pos.Clone().Distance(ahead.Clone()) <= obstacle.Radius ||
                   obstacle.Pos.Clone().Distance(aheadHalf.Clone()) <= obstacle.Radius ||
                   obstacle.Pos.Clone().Distance(ME.Pos.Clone()) <= obstacle.Radius
                ;
        }

        /// <summary>
        /// Finds the closest obstacle to the entity
        /// </summary>
        /// <param name="ahead"></param>
        /// <param name="aheadHalf"></param>
        /// <returns>null or closest obstacle</returns>
        private StaticEntity? FindClosestObstacle(Vector2D ahead, Vector2D aheadHalf)
        {
            StaticEntity closestObstacle = null;

            foreach (StaticEntity obstacle in ME.World.StaticEntities)
            {
                bool collision = LineIntersectsCircle(ahead, aheadHalf, obstacle);

                if (collision && (closestObstacle == null || ME.World.Witch.Pos.Clone().Distance(obstacle.Pos) <
                        ME.World.Witch.Pos.Clone().Distance(closestObstacle.Pos)))
                {
                    closestObstacle = obstacle;
                }
            }

            return closestObstacle;
        }
    }
}