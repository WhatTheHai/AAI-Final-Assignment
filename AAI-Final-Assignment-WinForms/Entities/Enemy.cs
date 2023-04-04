using AAI_Final_Assignment_WinForms.Goals;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities; 

public class Enemy : MovingEntity {
    private const int lowHealthThreshold = 40;
    private const int FleeingThreshold = 400;

    private readonly int maxStamina;

    public Enemy(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight, float mass,
        float maxSpeed, float maxForce, float radius, int stamina) : base(pos, world, scale, textureWidth,
        textureHeight,
        mass, maxSpeed,
        maxForce, radius) {
        // SteeringBehaviour.Arrive = false;
        SteeringBehaviour.Seek = false;
        SteeringBehaviour.Flee = false;
        SteeringBehaviour.ObstacleAvoidance = true;
        maxStamina = stamina;
        Stamina = stamina;

        MainGoal = new ThinkGoal(this);
        CurrentTarget = World.Witch;

        Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\sharkboy.png"),
            new Size(TextureWidth, TextureHeight));

        DetermineColor();
    }

    public Color Color { get; set; }
    public Goal MainGoal { get; set; }
    public int Damage { get; set; }
    public double Stamina { get; set; }

    public void DetermineColor() {
        //Max mass is 100
        var redValue = (int)Math.Ceiling(255 * (Mass / 100));
        Color = Color.FromArgb(255, Math.Clamp(redValue, 0, 255), 0, 0);
    }

    public override void Update(float timeElapsed) {
        if (Health <= 0) World.MovingEntities.Remove(this);

        MainGoal.Process();
        base.Update(timeElapsed);
    }

    public override void Render(Graphics g) {
        g.DrawEllipse(new Pen(Color, 3),
            new Rectangle((int)Pos.X - (int)Radius, (int)Pos.Y - (int)Radius, (int)Radius * 2, (int)Radius * 2));

        var drawFont = new Font("Arial", 10);
        var drawBrush = new SolidBrush(Color.Yellow);
        var x = Pos.X + 10;
        var y = Pos.Y + 10;
        var drawFormat = new StringFormat();


        if (World.ShowGoals) g.DrawString(MainGoal.Display(), drawFont, drawBrush, x, y, drawFormat);
        RenderHp(g);
        if (World.ShowForces) RenderForceArrows(g);
    }

    public bool HasNoStamina() {
        return Stamina <= 0;
    }

    public bool HasMaxStamina() {
        return Stamina >= maxStamina;
    }

    public bool HasLowHealth() {
        return Health < lowHealthThreshold;
    }

    public bool IsToCloseToTarget(MovingEntity target) {
        return Pos.Distance(target.Pos) < FleeingThreshold;
    }
}