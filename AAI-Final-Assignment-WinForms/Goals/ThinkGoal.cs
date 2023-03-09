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
        public ThinkGoal(string name, MovingEntity entity) : base(name, entity)
        {
        }

        public override void Activate()
        {
            GoalStatus = GoalStatusType.Active;
            SubGoalsStack.Clear();
            SubGoalsStack.Push(new WanderGoal("Wander", Owner));
            SubGoalsStack.Push(new SeekTargetGoal("Seek", Owner));
            SubGoalsStack.Push(new WanderGoal("Wander", Owner));
            SubGoalsStack.Push(new SeekTargetGoal("Seek", Owner));
        }


        public override void Process()
        {
            if (SubGoalsStack.Count == 0) Activate(); // todo: remove

            var currentGoal = SubGoalsStack.Peek();

            if (currentGoal.GoalStatus == GoalStatusType.Completed || currentGoal.GoalStatus == GoalStatusType.Failed)
            {
                SubGoalsStack.Pop().Deactivate();
            }

            if (SubGoalsStack.Count == 0) Activate(); //todo: remove 

            currentGoal = SubGoalsStack.Peek();
            currentGoal.Process();
        }

        public override void Deactivate()
        {
        }
    }
}