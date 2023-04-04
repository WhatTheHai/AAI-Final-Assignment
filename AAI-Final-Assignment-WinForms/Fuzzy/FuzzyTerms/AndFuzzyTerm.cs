namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms; 

public class AndFuzzyTerm : FuzzyTerm {
    public AndFuzzyTerm(FuzzyTerm t1) {
        terms.Add(t1);
    }

    public AndFuzzyTerm(FuzzyTerm t1, FuzzyTerm t2) : this(t1) {
        terms.Add(t2);
    }

    public AndFuzzyTerm(FuzzyTerm t1, FuzzyTerm t2, FuzzyTerm t3) : this(t1, t2) {
        terms.Add(t3);
    }

    public AndFuzzyTerm(AndFuzzyTerm term) {
        terms = term.terms;
    }


    public override FuzzyTerm Clone() {
        return new AndFuzzyTerm(this);
    }

    public override float GetDOM() {
        var lowest = float.MaxValue;
        foreach (var t in terms)
            if (t.GetDOM() < lowest)
                lowest = t.GetDOM();

        return lowest;
    }

    public override void ClearDom() {
        foreach (var t in terms) t.ClearDom();
    }

    public override void ORwithDOM(float value) {
        foreach (var t in terms) t.ORwithDOM(value);
    }
}