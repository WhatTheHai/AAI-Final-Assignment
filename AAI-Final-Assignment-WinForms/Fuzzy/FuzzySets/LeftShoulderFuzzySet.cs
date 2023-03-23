using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets
{
    // |  --------\
    // |           \
    // |            \
    // |             \
    // |              \
    // |_____________________
    public class LeftShoulderFuzzySet : FuzzySet
    {
        private float peakPoint;
        private float leftPoint;
        private float rightPoint;

        public LeftShoulderFuzzySet(float peak, float left, float right) : base(((peak + right) / 2))
        {
        }

        public override float CalculateDOM(float value)
        {
            // check for offset 0 
            if ((rightPoint == 0f && peakPoint == value) || (leftPoint == 0f && peakPoint == value)) return 1.0f;

            if ((value >= peakPoint) && (value < (peakPoint - rightPoint)))
            {
                float grad = 1.0f / -rightPoint;

                return grad * (value - peakPoint) + 1.0f;
            }

            if ((value < peakPoint) && (value >= peakPoint - leftPoint))
            {
                return 1.0f;
            }

            return 0.0f;
        }
    }
}