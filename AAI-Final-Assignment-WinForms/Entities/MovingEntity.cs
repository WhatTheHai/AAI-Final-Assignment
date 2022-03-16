using System.Numerics;
using AAI_Final_Assignment_WinForms.util;

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

        protected MovingEntity()
        {
            // later constructor met parameters bouwen 
        }

        public override void Update(double timeElapsed)
        {
            // update entity position and orientation  
        }
    }
}
