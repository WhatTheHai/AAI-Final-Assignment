using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;

namespace AAI_Final_Assignment_WinForms.Fuzzy; 

public class FuzzyModule {
    public List<FuzzyRule> rules; // public for test
    public Dictionary<string, FuzzyVariable> variables; // public for tests.

    public FuzzyModule() {
        variables = new Dictionary<string, FuzzyVariable>();
        rules = new List<FuzzyRule>();
    }

    //Creates a new "empty" fuzzy variable and returns a reference to it
    public FuzzyVariable CreateFLV(string name) {
        var fuzzyVariable = new FuzzyVariable();
        variables.Add(name, fuzzyVariable);
        return fuzzyVariable;
    }

    public FuzzyVariable GetFLV(string name) {
        return variables.ContainsKey(name) ? variables[name] : null;
    }

    //Adds a rule to the module
    public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequence) {
        rules.Add(new FuzzyRule(antecedent, consequence));
    }

    //This method calls the Fuzzify method of the named FLV
    public void Fuzzify(string nameOfFlv, float value) {
        if (variables.ContainsKey(nameOfFlv)) variables[nameOfFlv].Fuzzify(value);
    }

    //Given a fuzzy variable and a defuzzification method this returns a crisp value.
    public float DeFuzzify(string key) {
        if (variables.TryGetValue(key, out var fuzzyVariable)) {
            foreach (var rule in rules) rule.SetConfidenceOfConsequentToZero();

            foreach (var rule in rules) rule.Calculate();

            return fuzzyVariable.DeFuzzifyMaxAv();
        }

        return 0.0f;
    }
}