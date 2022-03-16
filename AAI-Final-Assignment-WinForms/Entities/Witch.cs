using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public class Witch : MovingEntity
    {
        public Color WColor { get; set; }

        public Witch(Vector2D pos, GameWorld w, float scale) : base(pos, w) {
            Velocity = new Vector2D(0, 0);
            Scale = scale;
            WColor = Color.Green;
        }
    }

}
