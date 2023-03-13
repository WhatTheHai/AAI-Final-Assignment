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

        //Perpendicular direction of entity
        public Vector2D Side { get; set; }

        // Mass of entity
        public float Mass { get; set; }

        //Maximum velocity of entity 
        public float MaxSpeed { get; set; }

        //Maximum force on entity
        public float MaxForce { get; set; }

        //Hitpoints
        public double MaxHealth { get; set; }

        //Current Health
        public double Health { get; set; }

        // used for debugging to show current steeringforce
        private Vector2D currentSteeringForce;


        // todo: not using TURNRATE
        //Maximum turn rate 
        public float MaxTurnRate { get; set; }

        public SteeringBehaviour SteeringBehaviour { get; set; }

        protected MovingEntity(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight,
            float mass, float maxSpeed, float maxForce, float radius
        ) : base(pos, world, scale, textureWidth, textureHeight, radius)
        {
            Mass = mass;
            MaxSpeed = maxSpeed;
            MaxForce = maxForce;
            Velocity = new Vector2D();
            Heading = new Vector2D();
            Side = new Vector2D();
            SteeringBehaviour = new SteeringBehaviour(this);
            currentSteeringForce = new Vector2D();
            MaxHealth = Health = 100;
        }

        public override void Update(float timeElapsed)
        {
            // calculate steering force
            Vector2D steeringForce = SteeringBehaviour.Calculate();
            currentSteeringForce = steeringForce.Clone();

            // acceleratie force/mass 
            Vector2D acceleration = steeringForce.Clone().Divide(Mass);

            // update velocity
            Velocity.Add(acceleration.Multiply(timeElapsed));
            //Velocity.Add(steeringForce);

            // dont exeed max velocity 
            Velocity.Truncate(MaxSpeed);

            // update position 
            Pos.Add(Velocity.Clone().Multiply(timeElapsed));

            // update heading and side if moving
            if (Velocity.LengthSquared() > 0.000000001)
            {
                Heading = Velocity.Clone().Normalize();
                Side = Heading.Clone().Perpendicular();
            }
        }


        public override void Render(Graphics g)
        {
            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            float x = (float)Pos.X;
            float y = (float)Pos.Y;
            StringFormat drawFormat = new StringFormat();

            g.DrawString($"Velocity: {Velocity.ToStringRounded()}", drawFont, drawBrush, x, y, drawFormat);
            g.DrawString($"Heading: {Heading.ToStringRounded()}", drawFont, drawBrush, x, y + 20, drawFormat);
            g.DrawString($"Seek forces: {SteeringBehaviour.CurrentSeek.ToStringRounded()}", drawFont,
                drawBrush, x, y + 40,
                drawFormat);
            g.DrawString($"Arrive forces: {SteeringBehaviour.CurrentArrive.ToStringRounded()}", drawFont,
                drawBrush, x, y + 60,
                drawFormat);
            g.DrawString($"Added forces: {currentSteeringForce.ToStringRounded()}", drawFont, drawBrush, x, y + 80,
                drawFormat);
            g.DrawString($"Obstacle forces: {SteeringBehaviour.CurrentObstacleAvoidance.ToStringRounded()}",
                drawFont,
                drawBrush, x, y + 100,
                drawFormat);

            g.DrawString(SteeringBehaviour.IsCollision ? $"Collision: Yes" : $"Collision: No", drawFont, drawBrush, x,
                y + 120,
                drawFormat);

            RenderInfo(g);
        }

        protected void RenderInfo(Graphics g)
        {
            int width = 3;
            float multiplier = 5;

            // current velocity
            // DrawLineFromEntity(g, Pos, Velocity.Clone().Multiply(multiplier), Color.Green, width);

            // desired seek force

            // DrawLineFromEntity(g, Pos, SteeringBehaviour.CurrentDesiredForceSeek.Clone().Multiply(multiplier),
            //     Color.Gray,
            //     width);

            // desired obstacle force
            // DrawLineFromEntity(g, Pos, SteeringBehaviour.CurrentDesiredForceObstacle.Clone().Multiply(multiplier),
            //     Color.Yellow,
            //     width);

            // steering force
            // DrawLineFromEntity(g, Pos, currentSteeringForce.Clone().Multiply(multiplier), Color.Red, width);
            // Vector2D currentSteeringForceEndPoint = currentVelocity.Clone().Add(currentSteeringForce.Clone());
            //DrawLineFromEntity(g, Pos, currentSteeringForceEndPoint, Color.Blue, width);
        }

        protected void DrawLineFromEntity(Graphics g, Vector2D start, Vector2D end, Color color, int width)
        {
            //g.DrawLine(new Pen(color, width), (int)start.X, (int)start.Y, (int)end.X, (int)end.Y);
            PointF startPoint = new PointF((float)start.X, (float)start.Y);
            PointF endPoint = new PointF((float)end.X + (float)start.X, (float)end.Y + (float)start.Y);

            g.DrawLine(new Pen(color, width), startPoint, endPoint);
        }
    }
}