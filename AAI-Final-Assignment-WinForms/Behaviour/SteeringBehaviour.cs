using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace AAI_Final_Assignment_WinForms.Behaviour
{
    public class SteeringBehaviour
    {
        // todo: get setters? initialize total/currentforce 
        // All steering behaviour that can be enabled
        public bool Seek, Flee, Arrive, ObstacleAvoidance, Wander;

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
        public Vector2D CurrentWander = new();

        public bool IsCollision { get; set; }

        /// <summary>
        /// Calculates all different forces together till max force of entity is reached
        /// </summary>
        /// <returns>The total force applied to the entity</returns>
        public Vector2D Calculate()
        {
            TotalForce = new Vector2D();
            // todo check.
            if (ObstacleAvoidance)
            {
                CurrentForce = CalculateObstacleAvoidance();
                if (!AccumulateForce(TotalForce, CurrentForce)) return TotalForce;
            }

            if (Flee)
            {
                CurrentForce = CalculateFlee();
                if (!AccumulateForce(TotalForce, CurrentForce)) return TotalForce;
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

            if (Wander)
            {
                CurrentForce = CalculateWander();
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
            float magnitudeSoFar = runningTotal.Length();
            float magnitudeRemaining = ME.MaxForce - magnitudeSoFar;
            if (magnitudeRemaining <= 0.0)
            {
                return false;
            }

            float magnitudeToAdd = forceToAdd.Length();

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
            Vector2D targetPos = ME.CurrentTarget.Pos.Clone();
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
            float panicDistanceSq = 100.0F;
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
            const float deceleration = 500;


            Vector2D mePos = ME.Pos.Clone();
            Vector2D targetPos = ME.World.Witch.Pos.Clone();
            Vector2D toTarget = targetPos.Sub(mePos);

            float dist = toTarget.Length();

            if (dist > 0.0001)
            {
                float speed = dist / deceleration;
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
            float maxSeeAhead = 100;
            // Calculate dynamic length
            float length = ME.Velocity.Clone().Length() / ME.MaxSpeed;

            //todo: max avoidance force, maybe use local
            Vector2D avoidanceForce = new Vector2D();

            // Max range vector
            Vector2D aheadVector = ME.Velocity.Clone().Normalize().Multiply(length).Multiply(maxSeeAhead)
                .Add(ME.Pos.Clone());
            // Half of the max range vector
            Vector2D aheadVectorHalf = ME.Velocity.Clone().Normalize().Multiply(length).Multiply(maxSeeAhead)
                .Multiply(0.5f).Add(ME.Pos.Clone());

            // Look for closest obstacle
            StaticEntity? closestObstacle = FindClosestObstacle(aheadVector, aheadVectorHalf);

            if (closestObstacle == null)
            {
                CurrentObstacleAvoidance = avoidanceForce.Clone();
                return avoidanceForce;
            }

            // calculate force
            avoidanceForce = aheadVector.Clone().Sub(closestObstacle.Pos.Clone());
            avoidanceForce.Normalize();
            avoidanceForce.Multiply(ME.MaxForce);

            CurrentObstacleAvoidance = avoidanceForce.Clone();

            return avoidanceForce;
        }

        /// <summary>
        /// Calculates the desired forces for wandering around randomly
        /// </summary>
        /// <returns>The wander force</returns>
        public Vector2D CalculateWander()
        {
            //Wander parameters
            //TODO: Scale with max speed
            const float wanderRadius = 15.0f;
            const float wanderDistance = 30.0f;
            const float wanderJitter = 0.5f;
            Random rand = new Random();

            Vector2D wanderTarget = ME.Heading.Clone();
            //Create a random displacement vector and add it to the wander target
            //Ensure the float number is between -1 and 1
            Vector2D displacement = new Vector2D(rand.NextSingle() * 2 - 1, rand.NextSingle() * 2 - 1);
            displacement.Multiply(wanderJitter);
            displacement.Normalize();
            displacement.Multiply(wanderDistance);
            wanderTarget.Add(displacement);

            // Calculate the target vector in local space, and set its length to the wander radius
            Vector2D targetLocal = wanderTarget.Sub(ME.Pos);
            targetLocal.Normalize().Multiply(wanderRadius);

            // Rotate the target vector by a random angle
            float angle = rand.NextSingle() * (float)Math.PI * 2;

            // Perform a rotation using a rotation matrix
            Vector2D targetWorld = new Vector2D(
                wanderTarget.X * MathF.Cos(angle) - wanderTarget.Y * MathF.Sin(angle),
                wanderTarget.X * MathF.Sin(angle) + wanderTarget.Y * MathF.Cos(angle)
            );

            Vector2D desiredForce = targetWorld.Sub(ME.Velocity);
            CurrentWander = desiredForce;
            return desiredForce;
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