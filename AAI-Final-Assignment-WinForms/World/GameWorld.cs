using AAI_Final_Assignment_WinForms.Behaviour;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.World
{
    public class GameWorld
    {
        private List<MovingEntity> _movingEntities;
        public List<ObstacleEntity> Obstacles;
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
            _movingEntities = new List<MovingEntity>();
            Obstacles = new List<ObstacleEntity>();
            Width = w;
            Height = h;

            Witch = new Witch(new Vector2D(100, 100), this, 5);
            Populate();
        }

        public void Update(float timeElapsed)
        {
            foreach (MovingEntity me in _movingEntities)
            {
                me.Update(timeElapsed);
            }
        }

        public void Render(Graphics g)
        {
            _movingEntities.ForEach(e => e.Render(g));
            Obstacles.ForEach(o => o.Render(g));
            Witch.Render(g);
        }

        private void Populate()
        {
            // for (int i = 0; i < 10; i++)
            // {
            //     TestEnemy t = new TestEnemy(new Vector2D(10, i * 10), this, 5);
            //     _movingEntities.Add(t);
            // }

            TestEnemy t = new TestEnemy(new Vector2D(10, 10), this, 5);
            _movingEntities.Add(t);

            Circle o = new Circle(new Vector2D(100, 100), this);
            Obstacles.Add(o);
        }
    }
}