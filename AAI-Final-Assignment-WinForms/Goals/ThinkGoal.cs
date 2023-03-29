using System.Diagnostics;
using System.Text;
using AAI_Final_Assignment_WinForms.Entities;
using AAI_Final_Assignment_WinForms.Goals.Abstracts;
using AAI_Final_Assignment_WinForms.Goals.Enums;


namespace AAI_Final_Assignment_WinForms.Goals
{
    /// <summary>
    /// This is the brain of the moving entity and can never be deactivated. 
    /// </summary>
    public class ThinkGoal : CompositeGoal
    {
        public ThinkGoal(MovingEntity entity) : base(entity)
        {
            Name = "Thinking";
        }

        public override void Activate()
        {
            GoalStatus = GoalStatusType.Active;
            SubGoalsStack.Clear();
            // SubGoalsStack.Push(new WanderGoal(Owner));
            // SubGoalsStack.Push(new SeekTargetGoal(Owner));
            // SubGoalsStack.Push(new MoveToItemGoal(Owner));
            // SubGoalsStack.Push(new WanderGoal(Owner));
            SelectNewGoal();
        }


        public override void Process()
        {
            if (!IsActive()) Activate();

            // check if their is a subgoal to be processed. 
            if (SubGoalsStack.Count > 0)
            {
                var currentGoal = SubGoalsStack.Peek();

                if (currentGoal != null)
                {
                    while (
                        currentGoal != null &&
                        (currentGoal.GoalStatus == GoalStatusType.Completed ||
                         currentGoal.GoalStatus == GoalStatusType.Failed) &&
                        SubGoalsStack.Count > 0)
                    {
                        SubGoalsStack.Pop().Deactivate();
                        if (SubGoalsStack.Count > 0)
                        {
                            currentGoal = SubGoalsStack.Peek();
                        }
                    }
                }

                if (SubGoalsStack.Count <= 0 || currentGoal == null) return;
                currentGoal = SubGoalsStack.Peek();
                currentGoal.Process();
            }
            else
            {
                SelectNewGoal();
            }
        }

        public override void Deactivate()
        {
            // thinking may not be deactivated else the entity has no brain.
        }

        private void SelectNewGoal()
        {
            Random random = new Random();
            const int numberOfGoals = 3;
            var number = random.Next(1, (numberOfGoals + 1));

            switch (number)
            {
                case 1:
                    SubGoalsStack.Push(new MoveToItemGoal(Owner));
                    break;
                case 2:
                    SubGoalsStack.Push(new WanderGoal(Owner));
                    break;
                case 3:
                    SubGoalsStack.Push(new AttackGoal(Owner));
                    break;
            }
        }
    }
}