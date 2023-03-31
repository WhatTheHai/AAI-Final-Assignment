using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets
{
    public abstract class FuzzySet
    {
        private float DOM;
        private float representativeValue;

        protected FuzzySet(float representativeValue)
        {
            DOM = 0.0f;
            this.representativeValue = representativeValue;
        }

        public abstract float CalculateDOM(float value);

        public void ORwithDOM(float value) {
            DOM = (value > DOM) ? value : DOM;
        }

        public float GetRepresentativeVal() { return representativeValue; }
        public float GetDOM() { return DOM; }
        public void SetDOM(float value) { DOM = value; }
        public void ClearDOM() { DOM = 0.0f; }
    }
}