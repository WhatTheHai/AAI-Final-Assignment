using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Fuzzy;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.World; 

/// <summary>
/// The GameWorld class contains all the data and objects pertinent to the environment
/// like: walls, obstacles, agents etc...
/// </summary>
public class GameWorld {
    public const string PathPrefix = "..\\..\\..\\";

    private readonly int amountOfEnemies = 4;

    private readonly Random rand;

    // Place to store the generated background
    private Bitmap background;

    // Preloaded background images
    public List<Bitmap> BackgroundImages = new();

    // Fuzzylogic module for the Enemy
    public EnemyModule EnemyModule;

    // The graph where the characters can move over.
    public Graph.Graph GameGraph;
    public bool GraphEnabled = false;

    // List of all items
    public List<BaseGameEntity> Items;

    // List of all moving entities
    public List<BaseGameEntity> MovingEntities;

    // List of all projectiles
    public List<BaseGameEntity> Projectiles;

    //show forces
    public bool ShowForces = false;

    // show goals
    public bool ShowGoals = false;

    // List of all static entities
    public List<BaseGameEntity> StaticEntities;

    public GameWorld(int w, int h) {
        rand = new Random();
        MovingEntities = new List<BaseGameEntity>();
        StaticEntities = new List<BaseGameEntity>();
        Items = new List<BaseGameEntity>();
        Projectiles = new List<BaseGameEntity>();
        BackgroundImages = new List<Bitmap>();
        EnemyModule = new EnemyModule();


        for (var i = 1; i < 9; i++) {
            var img = Image.FromFile(PathPrefix + $"Sprites\\Floors\\floor_{i}.png");
            var bmp = new Bitmap(img, img.Width, img.Height);
            BackgroundImages.Add(bmp);
        }

        Width = w;
        Height = h;

        SpawnWitch();
        Populate();
        GameGraph = new Graph.Graph(this);
    }

    // Current controllable entity
    public Witch Witch { get; set; }

    // Width of main panel 
    public int Width { get; set; }

    // Height of main panel
    public int Height { get; set; }

    //Timers for score
    public float ScoreTimer { get; set; }

    public float BestScore { get; set; }

    public void Update(float timeElapsed) {
        ScoreTimer += timeElapsed;
        //Spawns a new enemy once in a while
        if ((int)ScoreTimer % 750 == 0) SpawnEnemies(1);
        var MEandItems = GetMEandItems();
        foreach (MovingEntity me in MovingEntities.ToArray()) {
            //Prevents crashing
            if (me == null) continue;
            me.Update(timeElapsed);
            me.CheckCollisions(MEandItems);
            Boundary(me);
        }

        if (!Items.Any())
            SpawnItems(10);

        if (!MovingEntities.Any())
            SpawnEnemies(amountOfEnemies);

        if (Witch.IsDead()) {
            if (ScoreTimer > BestScore) BestScore = ScoreTimer;
            ScoreTimer = 0f;
            RefreshEnemies(amountOfEnemies);
            SpawnWitch();
        }

        Witch.Update(timeElapsed);
        Witch.CheckWithinRange(MEandItems);
    }

    public void SpawnWitch() {
        if (Witch == null) {
            Witch = new Witch(new Vector2D(10, 10), this, 2, 50, 50, 50, 5, 55, 25);
        }
        else {
            Witch.Pos = new Vector2D(10, 10);
            Witch.Health = Witch.MaxHealth;
        }
    }

    public void Render(Graphics g) {
        // Render priority 
        // Background -> Graph -> Entities -> Main Character
        RenderBackground(g);
        if (GraphEnabled) GameGraph.Render(g);

        foreach (var baseGameEntity in MovingEntities.ToArray()) {
            if (baseGameEntity == null) continue;
            baseGameEntity.Render(g);
        }

        StaticEntities.ForEach(o => o.Render(g));
        Items.ToList().ForEach(o => o.Render(g));
        Witch.Render(g);
        RenderLabels(g);
        RenderScores(g);
    }

    public List<BaseGameEntity> GetMEandItems() {
        var allEntities = new List<BaseGameEntity>(MovingEntities.Count + Items.Count);
        allEntities.AddRange(MovingEntities);
        allEntities.AddRange(Items);
        return allEntities;
    }

    public void SpawnProjectile(Vector2D pos, Vector2D heading) {
        var projectile = new Projectile(pos.Clone(), this, 1, 10, 10, 0, 5, 5, 3, heading);
        MovingEntities.Add(projectile);
    }

    public void RenderBackground(Graphics g) {
        if (background == null) // generate the background only once
        {
            var Rand = new Random();
            background = new Bitmap(Width, Height);
            var bg = Graphics.FromImage(background);
            for (var x = 0; x < Width; x += BackgroundImages[0].Width)
            for (var y = 0; y < Height; y += BackgroundImages[0].Height) {
                Bitmap floor;
                //Make floor0 more common
                floor = Rand.Next(0, 3) == 0
                    ? BackgroundImages[Rand.Next(BackgroundImages.Count)]
                    : BackgroundImages[0];
                bg.DrawImage(floor, x, y);
            }
        }

        g.DrawImage(background, 0, 0); // render the stored background
    }

    private void Boundary(MovingEntity entity) {
        if (entity.Pos.X < 0 || entity.Pos.X > Width) {
            entity.Velocity.X = -entity.Velocity.X; // Inverts the x velocity to bounce off the left or right edge
            entity.Pos.X =
                Math.Max(0, Math.Min(entity.Pos.X, Width)); // Clamps the position within the screen bounds
        }

        if (entity.Pos.Y < 0 || entity.Pos.Y > Height) {
            entity.Velocity.Y = -entity.Velocity.Y; // Same as x velocity, but with the y-axis instead
            entity.Pos.Y = Math.Max(0, Math.Min(entity.Pos.Y, Height));
        }
    }

    /// <summary>
    ///     Spawn all entities and to according list.
    /// </summary>
    private void Populate() {
        SpawnObstacles(20);
        SpawnItems(10);
        SpawnEnemies(amountOfEnemies);
    }

    private void SpawnObstacles(int amount) {
        var currentAmount = 0;

        while (currentAmount != amount) {
            var o1 = new Circle(new Vector2D(rand.Next(0, Width), rand.Next(0, Height)), this, 2, 25, 25,
                rand.Next(20, 70));
            if (o1.CheckAnyCollisions(StaticEntities)) {
                StaticEntities.Add(o1);
                currentAmount++;
            }
        }
    }

    private void SpawnEnemies(int amount) {
        var currentAmount = 0;

        var staticEntities = GetStaticEntities();

        while (currentAmount != amount) {
            var enemy = new Enemy(new Vector2D(rand.Next(0, Width), rand.Next(0, Height)), this, 1, 50, 50,
                rand.Next(10, 100), rand.Next(1, 15),
                50, rand.NextSingle() * (10 - 20) + 20, rand.Next(180, 220));
            DetermineDamage(enemy);
            if (enemy.CheckAnyCollisions(staticEntities)) {
                MovingEntities.Add(enemy);
                currentAmount++;
            }
        }
    }

    private void RefreshEnemies(int amount) {
        MovingEntities = new List<BaseGameEntity>();
        SpawnEnemies(amount);
    }

    private void DetermineDamage(Enemy enemy) {
        EnemyModule.FuzzyEnemyModule.Fuzzify("Speed", enemy.MaxSpeed);
        EnemyModule.FuzzyEnemyModule.Fuzzify("Mass", enemy.Mass);

        var damage = EnemyModule.FuzzyEnemyModule.DeFuzzify("Damage");
        enemy.Damage = (int)Math.Ceiling(damage);
    }

    private void SpawnItems(int amount) {
        var currentAmount = 0;

        var staticEntities = GetStaticEntities();

        while (currentAmount != amount) {
            var i = new ItemSpawn(new Vector2D(rand.Next(0, Width), rand.Next(0, Height)), this, 2, 5, 5, 10);
            if (i.CheckAnyCollisions(staticEntities)) {
                Items.Add(i);
                currentAmount++;
            }
        }
    }

    private void RenderLabels(Graphics g) {
        var drawFont = new Font("Arial", 10);
        var drawBrush = new SolidBrush(Color.Yellow);
        var x = Width - 150f;
        var y = 10f;
        var drawFormat = new StringFormat();
        var margin = 20;

        g.DrawString("Key bindings:", drawFont, drawBrush, x, y, drawFormat);
        g.DrawString("Show Graph  :  G", drawFont, drawBrush, x, y += margin, drawFormat);
        g.DrawString("Show Path    :  H", drawFont, drawBrush, x, y += margin, drawFormat);
        g.DrawString("Show Forces :  F", drawFont, drawBrush, x, y += margin, drawFormat);
        g.DrawString("Show Goals   :  T", drawFont, drawBrush, x, y += margin, drawFormat);
    }

    private void RenderScores(Graphics g) {
        var currentScore = $"Current Score: {ScoreTimer / 10:0} points";
        var bestScore = $"Best Score: {BestScore / 10:0} points";
        g.DrawString(currentScore, SystemFonts.DefaultFont, Brushes.White, new PointF(10, 10));
        g.DrawString(bestScore, SystemFonts.DefaultFont, Brushes.White, new PointF(10, 20));
    }

    private List<BaseGameEntity> GetStaticEntities() {
        var allEntities = new List<BaseGameEntity>();
        allEntities.AddRange(StaticEntities);
        allEntities.AddRange(Items);

        return allEntities;
    }
}