using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;

namespace AAI_Final_Assignment_WinForms.Fuzzy; 

public class FuzzyRule {
    private readonly FuzzyTerm antecedent;
    private readonly FuzzyTerm consequence;

    public FuzzyRule(FuzzyTerm antecedent, FuzzyTerm consequence) {
        this.antecedent = antecedent.Clone();
        this.consequence = consequence.Clone();
    }

    public void SetConfidenceOfConsequentToZero() {
        consequence.ClearDom();
    }

    public void Calculate() {
        consequence.ORwithDOM(antecedent.GetDOM());
    }
}