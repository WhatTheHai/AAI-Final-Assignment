﻿using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals;

/// <summary>
///     Run away from attacker, wander and regen stamina
/// </summary>
public class RestGoal : CompositeGoal {
    public RestGoal(Enemy entity) : base(entity) {
        Name = "Resting";
    }

    public override void Activate() {
        SubGoalsStack.Clear();
        SubGoalsStack.Push(new WanderGoal(Owner));
        SubGoalsStack.Push(new FleeGoal(Owner));
        SetActive();
    }
}