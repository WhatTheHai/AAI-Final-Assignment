using AAI_Final_Assignment_WinForms.Behaviour;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public abstract class MovingEntity : BaseGameEntity
    {
        // velocity of entity  
        public Vector2D Velocity { get; set; }

        // direction of entity
        public Vector2D Heading { get; set; }

        // Perpendicular direction of entity
        public Vector2D Side { get; set; }

        // Mass of entity
        public float Mass { get; set; }

        //Maximum speed of entity 
        public float MaxSpeed { get; set; }

        //Maximum force on entity
        public float MaxForce { get; set; }

        //Maximum turn rate 
        public float MaxTurnRate { get; set; }

        public SteeringBehaviour SteeringBehaviour { get; set; }

        protected MovingEntity(Vector2D pos, GameWorld world) : base(pos, world)
        {
            
            Mass = 10;
            MaxSpeed = 50;
            MaxForce = 100;
            Velocity = new Vector2D();
            Heading = new Vector2D();
            SteeringBehaviour = new SteeringBehaviour(this);

        }

        public override void Update(double timeElapsed)
        {
            // calculate steering force
            Vector2D steeringForce = SteeringBehaviour.Calculate().Clone();

            // acceleratie force/mass 
            Vector2D acceleration = steeringForce.Divide(Mass).Clone();

            Velocity.Add(acceleration.Multiply(timeElapsed).Clone());
            // dont exeed max velocity 
            Velocity.Truncate(MaxSpeed);

            // update position 
            Pos.Add(Velocity.Multiply(timeElapsed).Clone());

            // update heading and side if moving
            if (Velocity.LengthSquared() > 0.000000000001)
            {
                Heading = Velocity.Normalize().Clone();
                Side = Heading.Perpendicular().Clone();
            }
        }

        // todo: remove!
        // public void TagObstacles(double boxRadius)
        // {
        //     foreach (var obstacle in World.Obstacles)
        //     {
        //         // first always set tag to false
        //         obstacle.IsTagged = false;
        //
        //         // wat is dit 
        //         Vector2D to = obstacle.Pos.Sub(Pos).Clone();
        //
        //         //  boxradius from vehicle  boundingradius from obstacle 
        //         double range = boxRadius + BoundingRadius;
        //
        //         // ????? 
        //         if (to.LengthSquared() < range * range)
        //         {
        //             obstacle.IsTagged = true;
        //         }
        //     }
        // }
    }
}