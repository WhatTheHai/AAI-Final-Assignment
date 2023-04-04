using AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets;
using AAI_Final_Assignment_WinForms.Fuzzy.FuzzyTerms;

namespace AAI_Final_Assignment_WinForms.Fuzzy; 

public class FuzzyVariable {
    private float maxRange;
    private readonly Dictionary<string, FuzzySet> memberSets;
    private float minRange;

    public FuzzyVariable() {
        memberSets = new Dictionary<string, FuzzySet>();
        minRange = maxRange = 0;
    }

    private void AdjustRangeToFit(float min, float max) {
        minRange = min < minRange ? min : minRange;
        maxRange = max > maxRange ? max : maxRange;
    }

    public void AddFzSet(string setName, string setType, float minBound, float peak, float maxBound) {
        switch (setType) {
            case "LeftShoulder":
                AddLeftShoulderSet(setName, minBound, peak, maxBound);
                break;
            case "RightShoulder":
                AddRightShoulderSet(setName, minBound, peak, maxBound);
                break;
            case "Triangular":
                AddTriangularSet(setName, minBound, peak, maxBound);
                break;
            default:
                throw new ArgumentException("Invalid set type: " + setType);
        }
    }

    public FzSet AddLeftShoulderSet(string name, float minBound, float peak, float maxBound) {
        memberSets.Add(name, new LeftShoulderFuzzySet(peak, peak - minBound, maxBound - peak));
        AdjustRangeToFit(minBound, maxBound);
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
        if (value > maxRange || value < minRange) throw new ArgumentOutOfRangeException();

        foreach (var set in memberSets.Values) set.SetDOM(set.CalculateDOM(value));
    }

    public float DeFuzzifyMaxAv() {
        var bottom = 0.0f;
        var top = 0.0f;

        foreach (var set in memberSets.Values) {
            bottom += set.GetDOM();
            top += set.GetRepresentativeVal() * set.GetDOM();
        }

        //Do not divide by zero
        if (bottom.Equals(0.0f)) return 0.0f;

        return top / bottom;
    }
}