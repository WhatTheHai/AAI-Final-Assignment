using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public abstract class BaseGameEntity
    {
        // path prefix for loading sprites etc.
        public const string PathPrefix = "..\\..\\..\\";

        // position in world of entity 
        public Vector2D Pos { get; set; }

        // scale size of entity
        public double Scale { get; set; }

        // world that contains the entity
        public GameWorld World { get; set; }

        // records the next unique identifier for an entity 
        private static int _nextId;

        // unique identifier of entity 
        public int Id { get; set; }

        // bounding radius of a object
        public double Radius { get; set; }


        // texture setup 
        public int TextureWidth { get; set; }
        public int TextureHeight { get; set; }
        public Bitmap? Texture { get; set; }


        protected BaseGameEntity(Vector2D pos, GameWorld world, double scale, int textureWidth, int textureHeight,
            double radius)
        {
            Pos = pos;
            World = world;
            Scale = scale;
            Id = _nextId++;
            Radius = radius;
            TextureHeight = textureHeight * (int)scale;
            TextureWidth = textureWidth * (int)scale;
        }

        // every entity needs a update function
        public virtual void Update(double timeElapsed)
        {
        }

        public virtual void Render(Graphics g)
        {
            g.FillEllipse(Brushes.Blue, new Rectangle((int)Pos.X, (int)Pos.Y, 10, 10));
        }
    }
}