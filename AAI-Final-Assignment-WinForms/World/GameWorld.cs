using AAI_Final_Assignment_WinForms.Entities;

namespace AAI_Final_Assignment_WinForms.World
{
    public class GameWorld
    {
        private List<MovingEntity> entities = new List<MovingEntity>();
        public int Width { get; set; }
        public int Height { get; set; }
        // game world class
        // the game world class contains all the data and objects pertinent to the environment like: walls, obstacles, agents etc...

        // size/bounds of world 
        // render world 
        // update world 
        // later : graphs 

        public GameWorld(int width, int height) 
        {
            Width = width;
            Height = height;
        }
    }
}