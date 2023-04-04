using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities;

public abstract class BaseGameEntity {
    // path prefix for loading sprites etc.

    // records the next unique identifier for an entity 
    private static int _nextId;

    protected BaseGameEntity(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight,
        float radius) {
        Pos = pos;
        World = world;
        Scale = scale;
        Id = _nextId++;
        Radius = radius;
        TextureHeight = textureHeight * (int)scale;
        TextureWidth = textureWidth * (int)scale;
    }

    // position in world of entity 
    public Vector2D Pos { get; set; }

    // scale size of entity
    public float Scale { get; set; }

    // world that contains the entity
    public GameWorld World { get; set; }

    // unique identifier of entity 
    public int Id { get; set; }

    // bounding radius of a object
    public float Radius { get; set; }

    // texture setup 
    public int TextureWidth { get; set; }
    public int TextureHeight { get; set; }
    public Bitmap? Texture { get; set; }

    public bool CheckAnyCollisions(List<BaseGameEntity> entities) {
        foreach (var entity in entities)
            if (entity != this && entity.Pos.Distance(Pos) < entity.Radius + Radius)
                return false;

        return true;
    }

    // every entity needs a update function
    public virtual void Update(float timeElapsed) { }

    public virtual void Render(Graphics g) {
        g.FillEllipse(Brushes.Blue, new Rectangle((int)(Pos.X - 5d), (int)(Pos.Y - 5d), 10, 10));
    }
}