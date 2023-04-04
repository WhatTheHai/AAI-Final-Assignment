using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.util;

namespace AAI_Final_Assignment_WinForms.Goals
{
    /// <summary>
    /// flee away from attacker. accounts for flee distance set in enemy
    /// </summary>
    public class FleeGoal : AtomicGoal
    {
        public FleeGoal(Enemy entity) : base(entity)
        {
            Name = "Fleeing";
        }

        public override void Activate()
        {
            Owner.SteeringBehaviour.Flee = true;
            SetActive();
        }

        public override void Process()
        {
            SetActiveIfInactive();
            if (!Owner.IsToCloseToTarget(Owner.World.Witch)) Deactivate();
        }

        public override void Deactivate()
        {
            Owner.SteeringBehaviour.Flee = false;
            SetComplete();
        }
    }
}