using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities; 

public class ItemSpawn : BaseGameEntity {
    public ItemSpawn(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight, float radius) :
        base(pos, world, scale, textureWidth, textureHeight, radius) { }

    public override void Render(Graphics g) {
        g.FillEllipse(Brushes.Green, new Rectangle((int)(Pos.X - 1.5d), (int)(Pos.Y - 1.5d), 3, 3));
        g.DrawEllipse(new Pen(Color.Green, 3),
            new Rectangle((int)Pos.X - (int)Radius, (int)Pos.Y - (int)Radius, (int)Radius * 2, (int)Radius * 2));
        base.Render(g);
    }
}