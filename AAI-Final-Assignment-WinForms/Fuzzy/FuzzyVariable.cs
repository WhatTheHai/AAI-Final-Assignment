using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;

namespace AAI_Final_Assignment_WinForms.Fuzzy
{
    public class FuzzyVariable
    {
        private Dictionary<string, FuzzySet> memberSets;
        private float minRange;
        private float maxRange;

        public FuzzyVariable()
        {
            memberSets = new Dictionary<string, FuzzySet>();
            minRange = maxRange = 0;
        }

        private void AdjustRangeToFit(float min, float max) {
            minRange = (min < minRange) ? min : minRange;
            maxRange = (max > maxRange) ? max : maxRange;
        }

        public void AddFzSet(string setName, string setType, float minBound, float peak, float maxBound)
        {
            FuzzySet setToAdd;
            switch (setType)
            {
                case "LeftShoulder":
                    setToAdd = new LeftShoulderFuzzySet(peak, peak - minBound, maxBound - peak);
                    break;
                case "RightShoulder":
                    setToAdd = new RightShoulderFuzzySet(peak, peak - minBound, maxBound - peak);
                    break;
                case "Triangular":
                    setToAdd = new TriangleFuzzySet(peak, peak - minBound, maxBound - peak);
                    break;
                default:
                    throw new ArgumentException("Invalid set type: " + setType);
            }
            memberSets.Add(setName, setToAdd);
            AdjustRangeToFit(minBound, maxBound);
            //return new FzSet(memberSets[setName]);
        }

        public FzSet AddLeftShoulderSet(string name, float minBound, float peak, float maxBound) {
            memberSets.Add(name, new LeftShoulderFuzzySet(peak, peak - minBound, maxBound - peak));
            AdjustRangeToFit(minBound,maxBound);
            return new FzSet(memberSets[name]);
        }

        public FzSet AddRightShoulderSet(string name, float minBound, float peak, float maxBound) {
            memberSets.Add(name, new RightShoulderFuzzySet(peak, peak - minBound, maxBound - peak));
            AdjustRangeToFit(minBound, maxBound);
            return new FzSet(memberSets[name]);
        }

        public FzSet AddTriangularSet(string name, float minBound, float peak, float maxBound) {
            memberSets.Add(name, new TriangleFuzzySet(peak, peak - minBound, maxBound - peak));
            AdjustRangeToFit(minBound, maxBound);
            return new FzSet(memberSets[name]);
        }

        public FzSet GetFzSet(string name) {
            return memberSets.ContainsKey(name) ? new FzSet(memberSets[name]) : null;
        }

        public void Fuzzify(float value) {
            if (value > maxRange || value < minRange) {
                throw new ArgumentOutOfRangeException();
            }

            foreach (FuzzySet set in memberSets.Values) {
                set.SetDOM(set.CalculateDOM(value));
            }
        }

        public float DeFuzzifyMaxAv() {
            float bottom = 0.0f;
            float top = 0.0f;

            foreach (FuzzySet set in memberSets.Values) {
                bottom += set.GetDOM();
                top += set.GetRepresentativeVal() * set.GetDOM();
            }

            //Do not divide by zero
            if (bottom.Equals(0.0f)) {
                return 0.0f;
            }

            return top / bottom;
        }
    }
}