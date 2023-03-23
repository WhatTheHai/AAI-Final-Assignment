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
        private float peak;
        private float leftOffset;
        private float rightOffset;

        public RightShoulderFuzzySet(float peak, float left, float right) : base(((peak + right) + peak) / 2)
        {
            this.peak = peak;
            this.leftOffset = left;
            this.rightOffset = right;
        }

        public override float CalculateDOM(float value)
        {
            // check for offset 0 
            if ((rightOffset == 0f && peak == value) || (leftOffset == 0f && peak == value)) return 1.0f;

            if ((value <= peak) && (value > (peak - leftOffset)))
            {
                float grad = 1.0f / leftOffset;

                return grad * (value - (peak - leftOffset));
            }

            if ((value > peak) && (value <= peak + rightOffset))
            {
                return 1.0f;
            }

            return 0.0f;
        }
    }
}