namespace AAI_Final_Assignment_WinForms.Entities
{
    public abstract class MovingEntity : BaseGameEntity
    {
        // velocity
        // heading
        // side
        // mass
        // maxspeed
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
