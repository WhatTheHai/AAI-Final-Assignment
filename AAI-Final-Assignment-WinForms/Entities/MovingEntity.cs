using System.Numerics;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; }
        public Vector2D Heading { get; set; }
        // side
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        // maxforce
        // max rotation rate 

        public MovingEntity(Vector2D pos, GameWorld world): base(pos, world) {
            Mass = 10;
            MaxSpeed = 10;
            Velocity = new Vector2D();
            Heading = new Vector2D();
        }

        public override void Update(double timeElapsed)
        {
            // update entity position and orientation  
        }
    }
}
