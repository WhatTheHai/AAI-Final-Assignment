using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Fuzzy
{
    abstract class FuzzySet {
        private float DOM;
        private float RepresentativeValue;

        protected FuzzySet(float representativeValue) {
            DOM = 0.0f;
            RepresentativeValue = representativeValue;
        }

        public abstract float CalculateDOM(float value);

        public abstract float ORwithDOM(float value);


    }
}
