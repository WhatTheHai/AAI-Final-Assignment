using AAI_Final_Assignment_WinForms.Entities;

namespace AAI_Final_Assignment_WinForms.World
{
    public class GameWorld
    {

        private List<MovingEntity> entities = new List<MovingEntity>();
        public int Width { get; set; }
        public int Height { get; set; }

        private int a, b;
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
            a += 10;
            b += 10;
            //entities.ForEach(e => e.Render(g));
            g.FillEllipse(Brushes.Yellow, new Rectangle(a, b, 20, 20));
            //Target.Render(g);
        }

        private void Populate()
        {
            // nee
        }
    }
}