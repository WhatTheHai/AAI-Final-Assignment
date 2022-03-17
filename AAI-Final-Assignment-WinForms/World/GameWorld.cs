using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.World
{
    public class GameWorld
    {
        private List<MovingEntity> entities = new List<MovingEntity>();
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
            Width = w;
            Height = h;
            Populate();
        }

        public void Update(float timeElapsed)
        {
            foreach (MovingEntity me in entities)
            {
                // me.SB = new SeekBehaviour(me); // restore later
                me.Update(timeElapsed);
            }
        }

        public void Render(Graphics g)
        {
            entities.ForEach(e => e.Render(g));
            Witch.Render(g);
        }

        private void Populate()
        {
            Witch = new Witch(new Vector2D(10, 10), this, 5);
            Witch.WColor = Color.BlueViolet;
        }
    }
}