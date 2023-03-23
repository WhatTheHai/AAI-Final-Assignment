using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;

namespace AAI_Final_Assignment_WinForms.Fuzzy
{
    public class FuzzyModule {
        private Dictionary<string, FuzzyVariable> variables;
        private List<FuzzyRule> rules;

        public FuzzyModule() {
            variables = new Dictionary<string, FuzzyVariable>();
            rules = new List<FuzzyRule>();
        }

        //Creates a new "empty" fuzzy variable and returns a reference to it
        public FuzzyVariable CreateFLV(string name) {
            FuzzyVariable fuzzyVariable = new FuzzyVariable();
            variables.Add(name, fuzzyVariable);
            return fuzzyVariable;
        }

        //Adds a rule to the module
        public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequence) {
            rules.Add(new FuzzyRule(antecedent, consequence));
        }

        //This method calls the Fuzzify method of the named FLV
        public void Fuzzify(string nameOfFlv, float value) {
            if (variables.ContainsKey(nameOfFlv)) {
                variables[nameOfFlv].Fuzzify(value);
            }
        }

        //Given a fuzzy variable and a defuzzification method this returns a crisp value.
        public float DeFuzzify(string key) {
            if (variables.TryGetValue(key, out FuzzyVariable fuzzyVariable)) {
                foreach (FuzzyRule rule in rules) {
                    rule.SetConfidenceOfConsequentToZero();
                    rule.Calculate();
                }
                //TODO: Add this
                //return fuzzyVariable.DeFuzzifyMaxAv();
                return 0.0f;
            }
            return 0.0f;
        }
    }
}
