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
        public double Mass { get; set; }

        //Maximum velocity of entity 
        public double MaxSpeed { get; set; }

        //Maximum force on entity
        public double MaxForce { get; set; }

        //Hitpoints
        public double MaxHealth { get; set; }

        //Current Health
        public double Health { get; set; }

        // used for debugging to show current steeringforce
        private Vector2D currentSteeringForce;


        // todo: not using TURNRATE
        //Maximum turn rate 
        public double MaxTurnRate { get; set; }

        public SteeringBehaviour SteeringBehaviour { get; set; }

        protected MovingEntity(Vector2D pos, GameWorld world, double scale, int textureWidth, int textureHeight,
            double mass, double maxSpeed, double maxForce, double radius
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

        public override void Update(double timeElapsed)
        {
            // calculate steering force
            Vector2D steeringForce = SteeringBehaviour.Calculate();
            currentSteeringForce = steeringForce.Clone();

            // acceleratie force/mass 
            Vector2D acceleration = steeringForce.Clone().Divide(Mass);

            // update velocity
            Velocity.Add(acceleration.Multiply(timeElapsed));
            //Velocity.Add(steeringForce);

            // dont exceed max velocity 
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

        public void CheckCollisions(List<BaseGameEntity> entities)
        {
            foreach (BaseGameEntity entity in entities)
            {
                if (entity is ItemSpawn item && item.Pos.Clone().Sub(Pos).Length() < item.Radius + Radius)
                {
                    if (this is not Projectile) {
                        // heals
                        World.Items.Remove(item);
                        if (Health + 10 >= MaxHealth) {
                            Health = MaxHealth;
                        }
                        else {
                            Health += 10;
                        }
                    }
                        
                }

                if (entity is Projectile projectile && this is TestEnemy &&
                    projectile.Pos.Clone().Sub(Pos.Clone()).Length() < projectile.Radius + Radius) {
                    World.MovingEntities.Remove(projectile);
                    if (Health - 10 <= 0) {
                        Health = 0;
                    }
                    else {
                        Health -= 10;
                    }
                }
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

            RenderHp(g);
            RenderInfo(g);
        }

        protected void RenderHp(Graphics g) {
            // Draw the health bar
            int healthBarWidth = (int)Radius*2;
            int healthBarHeight = 4;
            int healthBarX = (int)Pos.X - healthBarWidth / 2;
            int healthBarY = (int)Pos.Y - (int)Radius - healthBarHeight;
            int healthBarMaxWidth = healthBarWidth;

            // Calculate the width of the health bar based on the object's health
            double healthPercent = Health / MaxHealth;
            int healthBarCurrentWidth = (int)(healthPercent * healthBarMaxWidth);

            // Background of the health bar
            g.FillRectangle(Brushes.Gray, healthBarX, healthBarY, healthBarMaxWidth, healthBarHeight);
            // Current health
            g.FillRectangle(Brushes.Green, healthBarX, healthBarY, healthBarCurrentWidth, healthBarHeight);
            // Border of the health bar
            g.DrawRectangle(Pens.Black, healthBarX, healthBarY, healthBarMaxWidth, healthBarHeight);
        }

        protected void RenderInfo(Graphics g)
        {
            int width = 3;
            double multiplier = 5;

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