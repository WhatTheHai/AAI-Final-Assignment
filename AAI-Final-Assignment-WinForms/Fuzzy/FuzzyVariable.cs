using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets;

namespace AAI_Final_Assignment_WinForms.Fuzzy
{
    public class FuzzyVariable {
        private Dictionary<string, FuzzySet> memberSets;
        private float minRange;
        private float maxRange;

        public FuzzyVariable() {
            memberSets = new Dictionary<string, FuzzySet>();
            minRange = maxRange = 0;
        }

        public void AdjustRangeToFit(float min, float max) {
            minRange = (min < minRange) ? min : minRange;
            maxRange = (max > maxRange) ? max : maxRange;
        }

        pub

        public void Fuzzify(float value) {

        }
    }

}
