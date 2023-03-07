using AAI_Final_Assignment_WinForms.Behaviour;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.World
{
    public class GameWorld
    {
        // List of all moving entities
        public List<MovingEntity> MovingEntities;

        // List of all static entities
        public List<StaticEntity> StaticEntities;

        // The graph where the characters can move over.
        public Graph.Graph GameGraph;
        public bool GraphEnabled = false;

        // Current controllable entity
        public Witch Witch { get; set; }

        // Width of main panel 
        public int Width { get; set; }

        // Height of main panel
        public int Height { get; set; }

        // game world class
        // the game world class contains all the data and objects pertinent to the environment like: walls, obstacles, agents etc...

        // size/bounds of world 
        // render world 
        // update world 
        // later : graphs 

        public GameWorld(int w, int h)
        {
            MovingEntities = new List<MovingEntity>();
            StaticEntities = new List<StaticEntity>();
            Width = w;
            Height = h;

            Witch = new Witch(new Vector2D(10, 10), this, 1, 50, 50, 30, 100, 50, 50);
            Populate();
            GameGraph = new Graph.Graph(this);
        }

        public void Update(double timeElapsed)
        {
            foreach (MovingEntity me in MovingEntities)
            {
                me.Update(timeElapsed);
                Boundary(me);
            }
            Witch.Update(timeElapsed);
        }

        public void Render(Graphics g)
        {
            if (GraphEnabled)
            {
                GameGraph.Render(g);
            }

            MovingEntities.ForEach(e => e.Render(g));
            StaticEntities.ForEach(o => o.Render(g));
            Witch.Render(g);
        }

        private void Boundary(MovingEntity entity) {
            if (entity.Pos.X < 0 || entity.Pos.X > Width)
            {
                entity.Velocity.X = -entity.Velocity.X; // Inverts the x velocity to bounce off the left or right edge
                entity.Pos.X = Math.Max(0, Math.Min(entity.Pos.X, Width)); // Clamps the position within the screen bounds
            }
            if (entity.Pos.Y < 0 || entity.Pos.Y > Height)
            {
                entity.Velocity.Y = -entity.Velocity.Y; // Same as x velocity, but with the y-axis instead
                entity.Pos.Y = Math.Max(0, Math.Min(entity.Pos.Y, Height));
            }
        }

        /// <summary>
        /// Spawn all entities and to according list.
        /// </summary>
        private void Populate()
        {
            // for (int i = 0; i < 10; i++)
            // {
            //     TestEnemy t = new TestEnemy(new Vector2D(100, 100 + (i * 100)), this, 1, 50, 50, 50, 5, 10000); // 50 5 100000
            //     MovingEntities.Add(t);
            // }


            TestEnemy t = new TestEnemy(new Vector2D(1000, 1000), this, 1, 50, 50, 50, 5, 55, 25); // 50 5 100000
            MovingEntities.Add(t);

            Circle o = new Circle(new Vector2D(200, 250), this, 2, 30, 25, 25, 60);
            StaticEntities.Add(o);
            //
            Circle o2 = new Circle(new Vector2D(350, 400), this, 2, 30, 25, 25, 60);
            StaticEntities.Add(o2);
            // //
            Circle o3 = new Circle(new Vector2D(200, 350), this, 2, 30, 25, 25, 60);
            StaticEntities.Add(o3);
            // //
            Circle o4 = new Circle(new Vector2D(300, 250), this, 2, 30, 25, 25, 60);
            StaticEntities.Add(o4);
            // //
            // Circle o5 = new Circle(new Vector2D(600, 350), this, 2, 60, 50, 50);
            // StaticEntities.Add(o5);
            //
            // Circle o6 = new Circle(new Vector2D(400, 200), this, 2, 12, 50, 50);
            // StaticEntities.Add(o6);
        }
    }
}