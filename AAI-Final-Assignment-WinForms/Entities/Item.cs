using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Item : StaticEntity
    {
        public Item(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight, double radius) :
            base(pos, world, scale, textureWidth, textureHeight, radius) {

        }
    }
}
