using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities; 

public class Circle : StaticEntity {
    public Circle(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight,
        float radius) :
        base(pos, world, scale, textureWidth, textureHeight, radius) {
        Texture = new Bitmap(Image.FromFile(PathPrefix + "Sprites\\CAULDRON FILL.png"),
            new Size(TextureWidth, TextureHeight));
    }


    public override void Render(Graphics g) {
     
        g.FillEllipse(Brushes.Purple,
            new Rectangle((int)Pos.X - (int)Radius, (int)Pos.Y - (int)Radius, (int)Radius * 2, (int)Radius * 2));
    }
}