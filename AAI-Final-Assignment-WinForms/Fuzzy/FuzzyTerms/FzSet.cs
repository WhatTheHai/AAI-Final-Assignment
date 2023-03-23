using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets;

namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms
{
    public class FzSet : FuzzyTerm {
        private FuzzySet set;

        public FzSet(FuzzySet set) {
            this.set = set;
        }
        public override FuzzyTerm Clone() {
            return new FzSet(set);
        }

        public override float GetDOM() {
            return set.GetDOM();
        }

        public override void ClearDom() {
            set.ClearDOM();
        }

        public override void ORwithDOM(float value) {
            set.ORwithDOM(value);
        }
    }
}
