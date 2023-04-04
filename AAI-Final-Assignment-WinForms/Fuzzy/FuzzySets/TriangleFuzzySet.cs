namespace AAI_Final_Assignment_WinForms.Fuzzy.FuzzySets; 
// |        /\
// |       /  \
// |      /    \
// |     /      \
// |    /        \
// |_____________________

public class TriangleFuzzySet : FuzzySet {
    private readonly float leftOffset;
    private readonly float peak;
    private readonly float rightOffset;

    public TriangleFuzzySet(float mid, float left, float right) : base(mid) {
        peak = mid;
        leftOffset = left;
        rightOffset = right;
    }

    public override float CalculateDOM(float value) {
        // check for divide by zero's
        if ((rightOffset == 0f && peak == value) || (leftOffset == 0f && peak == value)) return 1.0f;

        // get dom left 
        if (value <= peak && value >= peak - leftOffset) {
            var grad = 1.0f / leftOffset;
            return grad * (value - (peak - leftOffset));
        }

        if (value > peak && value < peak + rightOffset) {
            var grad = 1.0f / -rightOffset;
            return grad * (value - peak) + 1f;
        }

        return 0.0f;
    }
}