using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities
{
    public  class ObstacleEntity : BaseGameEntity 
    {
        public bool IsTagged { get; set; }
        public ObstacleEntity(Vector2D pos, GameWorld world) : base(pos, world)
        {

        }
   

    }
}
