namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets; 

// |  --------\
// |           \
// |            \
// |             \
// |              \
// |_____________________
public class LeftShoulderFuzzySet : FuzzySet {
    private readonly float leftOffset;
    private readonly float peak;
    private readonly float rightOffset;

    public LeftShoulderFuzzySet(float peak, float left, float right) : base((peak - left + peak) / 2) {
        this.peak = peak;
        leftOffset = left;
        rightOffset = right;
    }

    public override float CalculateDOM(float value) {
        // check for offset 0 
        if ((rightOffset == 0f && peak == value) || (leftOffset == 0f && peak == value)) return 1.0f;

        if (value >= peak && value < peak + rightOffset) {
            var grad = 1.0f / -rightOffset;

            return grad * (value - peak) + 1.0f;
        }

        if (value < peak && value >= peak - leftOffset) return 1.0f;

        return 0.0f;
    }
}