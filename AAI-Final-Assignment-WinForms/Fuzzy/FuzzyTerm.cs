using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Fuzzy
{
    public abstract class FuzzyTerm {
        public abstract FuzzyTerm Clone();

        public abstract float GetDOM();

        public abstract void ClearDom();

        public abstract void ORwithDOM(float value);
    }
}
