using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets
{
    // |        /-----------
    // |       /  
    // |      /    
    // |     /      
    // |    /        
    // |_____________________

    public class RightShoulderFuzzySet : FuzzySet
    {
        private float peakPoint;
        private float leftPoint;
        private float rightPoint;

        public RightShoulderFuzzySet(float peak, float left, float right) : base(((peak + right) + peak) / 2)
        {
        }

        public override float CalculateDOM(float value)
        {
            // check for offset 0 
            if ((rightPoint == 0f && peakPoint == value) || (leftPoint == 0f && peakPoint == value)) return 1.0f;

            if ((value <= peakPoint) && (value > (peakPoint - leftPoint)))
            {
                float grad = 1.0f / leftPoint;

                return grad * (value - (peakPoint - leftPoint));
            }

            if ((value > peakPoint) && (value <= peakPoint + rightPoint))
            {
                return 1.0f;
            }

            return 0.0f;
        }
    }
}