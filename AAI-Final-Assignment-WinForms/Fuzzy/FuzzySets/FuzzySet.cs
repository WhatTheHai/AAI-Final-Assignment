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

        public virtual float GetRepresentativeVal() { return representativeValue; }
        public virtual float GetDOM() { return DOM; }
        public virtual void SetDOM(float value) { DOM = value; }
        public virtual void ClearDOM() { DOM = 0.0f; }
    }
}