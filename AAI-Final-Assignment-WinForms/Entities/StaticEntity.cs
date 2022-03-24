using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class StaticEntity : BaseGameEntity
    {
        public int TextureWidth { get; set; }
        public int TextureHeight { get; set; }

        public StaticEntity(Vector2D pos, GameWorld world, int w, int h) : base(pos, world) {
            TextureWidth = w;
            TextureHeight = h;
        }
    }
}
