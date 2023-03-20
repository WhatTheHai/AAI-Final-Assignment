using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI_Final_Assignment_WinForms.Fuzzy
{
    public class FuzzyModule {
        private Dictionary<string, FuzzyVariable> variables;
        private List<FuzzyRule> rules;

        public FuzzyModule() {
            variables = new Dictionary<string, FuzzyVariable>();
            rules = new List<FuzzyRule>();
        }

        public FuzzyVariable CreateFLV(string name) {
            return null;
        }

        public void AddRule(FuzzyRule antecedent, FuzzyTerm consequence) {
            
        }

        public void Fuzzify(string nameOfFlv, float value) {

        }

        public double DeFuzzify(string key) {
            return 0;
        }
    }
}
