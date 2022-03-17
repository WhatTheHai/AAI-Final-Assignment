using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public abstract class BaseGameEntity
    {
        // position in world of entity 
        public Vector2D Pos { get; set; }
        // scale size of entity
        public float Scale { get; set; }
        // world that contains the entity
        public GameWorld World { get; set; }
        // radius where entity checks for obstacles
        public Vector2D BoundingRadius { get; set; }


        // todo: not sure if necessary :
        // records the next unique identifier for an entity 
        private static int _nextId;
        // unique identifier of entity 
        public int Id { get; set; }

        protected BaseGameEntity(Vector2D pos, GameWorld world)
        {
            Pos = pos;
            World = world;
            Id = _nextId++;
            // later constructor met parameters bouwen 
        }

        // every entity needs a update function
        public virtual void Update(double timeElapsed)
        {

        }

        public virtual void Render(Graphics g)
        {
            g.FillEllipse(Brushes.Blue, new Rectangle((int)Pos.X, (int)Pos.Y, 10, 10));
        }

        // can add other stats like: 

            // vector position
            // scaling flout
            // bounding radius float ?
            // model? 
            // rendering? 
    }
}