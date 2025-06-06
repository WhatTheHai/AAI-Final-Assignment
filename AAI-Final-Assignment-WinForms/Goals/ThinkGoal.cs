﻿using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;

namespace AAI_Final_Assignment_WinForms.Goals;

/// <summary>
///     This is the brain of the moving entity and can never be deactivated.
///     current choices are:
///     ThinkingGoal(composite): attacking/healing/resting
///     AttackGoal(composite): SelectWitchAsTargetGoal(atomic) + SeekTargetGoal(atomic)
///     HealGoal(composite): Flee(atomic) + selectItemGoal(atomic) + seekItemGoal(atomic)
///     RestGoal(composite): Flee(atomic) + RegenStamina(atomic)
/// </summary>
public class ThinkGoal : CompositeGoal {
    public ThinkGoal(Enemy entity) : base(entity) {
        Name = "Thinking";
    }

    public override void Activate() {
        SetActive();
        SubGoalsStack.Clear();
        SelectNewGoal();
    }

    public override void Process() {
        SetActiveIfInactive();
        // extra checks for stack and a current goal that is null because of a null exception sometimes
        if (SubGoalsStack.Count > 0)
            ProcessStack();
        else
            SelectNewGoal();
    }

    public override void Deactivate() {
        // thinking may not be deactivated else the entity has no brain.
    }

    /// <summary>
    ///     Randomly select what to do next?
    /// </summary>
    private void SelectNewGoal() {
        if (Owner.HasLowHealth())
            SubGoalsStack.Push(new HealGoal(Owner));
        else if (Owner.HasNoStamina()) SubGoalsStack.Push(new RestGoal(Owner));
        else SubGoalsStack.Push(new AttackGoal(Owner));
    }
}