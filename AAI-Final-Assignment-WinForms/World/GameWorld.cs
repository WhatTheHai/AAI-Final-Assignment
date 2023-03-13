using AAI_Final_Assignment_WinForms.Behaviour;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.World
{
    public class GameWorld
    {
        // List of all moving entities
        public List<BaseGameEntity> MovingEntities;

        // List of all static entities
        public List<BaseGameEntity> StaticEntities;
        
        // List of all items
        public List<BaseGameEntity> Items;

        // Preloaded background images
        public List<Bitmap> BackgroundImages = new List<Bitmap>();
        
        // Place to store the generated background
        private Bitmap background;

        // The graph where the characters can move over.
        public Graph.Graph GameGraph;
        public bool GraphEnabled = false;

        // Current controllable entity
        public Witch Witch { get; set; }

        // Width of main panel 
        public int Width { get; set; }

        // Height of main panel
        public int Height { get; set; }

        public const string PathPrefix = "..\\..\\..\\";


        // game world class
        // the game world class contains all the data and objects pertinent to the environment like: walls, obstacles, agents etc...

        // size/bounds of world 
        // render world 
        // update world 
        // later : graphs 

        public GameWorld(int w, int h)
        {
            for (int i = 1; i < 9; i++) {
                Image img = Image.FromFile(PathPrefix + $"Sprites\\Floors\\floor_{i}.png");
                Bitmap bmp = new Bitmap(img, img.Width, img.Height);
                BackgroundImages.Add(bmp);
            }

            MovingEntities = new List<BaseGameEntity>();
            StaticEntities = new List<BaseGameEntity>();
            Items = new List<BaseGameEntity>();
            Width = w;
            Height = h;

            Witch = new Witch(new Vector2D(10, 10), this, 1, 50, 50, 30, 100, 50, 25);
            Populate();
            GameGraph = new Graph.Graph(this);
        }

        public void Update(double timeElapsed) {
            List<BaseGameEntity> MEandItems = GetMEandItems();
            foreach (MovingEntity me in MovingEntities)
            {
                me.Update(timeElapsed);
                me.CheckCollisions(MEandItems, this);
                Boundary(me);
            }
            Witch.Update(timeElapsed);
            Witch.CheckWithinRange(MEandItems, this);
        }

        public void Render(Graphics g)
        {
            // Render priority 
            // Background -> Graph -> Entities -> Main Character
            RenderBackground(g);
            if (GraphEnabled)
            {
                GameGraph.Render(g);
            }

            MovingEntities.ForEach(e => e.Render(g));
            StaticEntities.ForEach(o => o.Render(g));
            Items.ForEach(o => o.Render(g));
            Witch.Render(g);
        }

        public List<BaseGameEntity> GetMEandItems() {
            List<BaseGameEntity> allEntities = new List<BaseGameEntity>();
            allEntities.AddRange(MovingEntities);
            allEntities.AddRange(Items);
            return allEntities;
        }

        public void RenderBackground(Graphics g)
        {
            if (background == null)  // generate the background only once
            {
                Random Rand = new Random();
                background = new Bitmap(Width, Height);
                Graphics bg = Graphics.FromImage(background);
                for (int x = 0; x < Width; x += BackgroundImages[0].Width)
                {
                    for (int y = 0; y < Height; y += BackgroundImages[0].Height) {
                        Bitmap floor;
                        //Make floor0 more common
                        floor = Rand.Next(0, 3) == 0 ? BackgroundImages[Rand.Next(BackgroundImages.Count)] : BackgroundImages[0];
                        bg.DrawImage(floor, x, y);
                    }
                }
            }

            g.DrawImage(background, 0, 0);  // render the stored background
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

            TestEnemy t = new TestEnemy(new Vector2D(1000, 1000), this, 1, 50, 50, 50, 5, 55, 12.5); // 50 5 100000
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
            SpawnItems();
            // //
            // Circle o5 = new Circle(new Vector2D(600, 350), this, 2, 60, 50, 50);
            // StaticEntities.Add(o5);
            //
            // Circle o6 = new Circle(new Vector2D(400, 200), this, 2, 12, 50, 50);
            // StaticEntities.Add(o6);
        }

        private void SpawnItems() 
        {
            Random Rand = new Random();
            int maxAmount = 10;
            int currentAmount = 0;

            List<BaseGameEntity> allEntities = new List<BaseGameEntity>();
            allEntities.AddRange(StaticEntities);
            allEntities.AddRange(Items);

            while (currentAmount != maxAmount) {
                ItemSpawn i = new ItemSpawn(new Vector2D(Rand.Next(0, Width), Rand.Next(0, Height)), this, 2, 5, 5, 10);
                if (i.CheckAnyCollisions(allEntities)) {
                    Items.Add(i);
                    currentAmount++;
                }
            }
        }
    }
}