using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets
{
    // |        /\
    // |       /  \
    // |      /    \
    // |     /      \
    // |    /        \
    // |_____________________

    public class TriangleFuzzySet : FuzzySet
    {
        private float peakPoint;
        private float leftPoint;
        private float rightPoint;

        public TriangleFuzzySet(float mid, float left, float right) : base(mid)
        {
            peakPoint = mid;
            leftPoint = left;
            rightPoint = right;
        }

        public override float CalculateDOM(float value)
        {
            // check for divide by zero's   todo: wat gebeured hier? 
            if ((rightPoint == 0f && peakPoint == value) || (leftPoint == 0f && peakPoint == value)) return 1.0f;

            // get dom left 
            if (value <= peakPoint && value >= (peakPoint - leftPoint))
            {
                float grad = 1.0f / leftPoint;
                return grad * (value - (peakPoint - leftPoint));
            }

            if (value > peakPoint && value < (peakPoint + rightPoint))
            {
                float grad = 1.0f / rightPoint;
                return grad * (value - peakPoint) + 1f;
            }

            return 0.0f;
        }
    }
}