using AAI_Final_Assignment_WinForms.Behaviour;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.World
{
    public class GameWorld
    {
        public List<MovingEntity> MovingEntities;
        public List<StaticEntity> StaticEntities;
        public Graph.Graph GameGraph;
        public bool GraphEnabled = false;
        public Witch Witch { get; set; }
        public int Width { get; set; }
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

            Witch = new Witch(new Vector2D(10, 10), this, 1, 50, 50, 0, 1000, 2, 50);
            Populate();
            GameGraph = new Graph.Graph(this);
        }

        public void Update(double timeElapsed)
        {
            foreach (MovingEntity me in MovingEntities)
            {
                me.Update(timeElapsed);
            }
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