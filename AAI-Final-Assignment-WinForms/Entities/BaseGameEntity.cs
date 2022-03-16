namespace AAI_Final_Assignment_WinForms.Entities
{
    public abstract class BaseGameEntity
    {
        // records the next unique identifier for an entity 
        private static int _nextId;

        // unique identifier of entity 
        public int Id { get; set; }

        protected BaseGameEntity()
        {
            Id = _nextId;
            _nextId++;
            // later constructor met parameters bouwen 
        }

        // every entity needs a update function
        public virtual void Update(double timeElapsed)
        {
        }

        // can add other stats like: 

        // vector position
        // scaling flout
        // bounding radius float ?
        // model? 
        // rendering? 
    }
}