namespace AAI_Final_Assignment_WinForms.util; 

public class Vector2D : IEquatable<Vector2D> {
    public Vector2D() : this(0, 0) { }

    public Vector2D(float x, float y) {
        X = x;
        Y = y;
    }
    
    public float X { get; set; }
    public float Y { get; set; }

    public bool Equals(Vector2D? other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public float Length() {
        return MathF.Sqrt(X * X + Y * Y);
    }

    public float LengthSquared() {
        return X * X + Y * Y;
    }


    public Vector2D Add(Vector2D v) {
        X += v.X;
        Y += v.Y;
        return this;
    }

    public Vector2D Sub(Vector2D v) {
        X -= v.X;
        Y -= v.Y;
        return this;
    }

    public Vector2D Multiply(float value) {
        X *= value;
        Y *= value;
        return this;
    }

    public Vector2D Divide(float value) {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), $@"Value cannot be 0 or lower, value is {value}");

        X /= value;
        Y /= value;
        return this;
    }
    
    public float Dot(Vector2D v2) {
        X *= v2.X;
        Y *= v2.Y;
        return X + Y;
    }

    public Vector2D Normalize() {
        var currentLength = Length();
        if (currentLength == 0) return new Vector2D(0, 0);
        X /= currentLength;
        Y /= currentLength;
        return this;
    }

    public Vector2D Truncate(float max) {
        if (Length() > max) {
            Normalize();
            Multiply(max);
        }

        return this;
    }

    public Vector2D Perpendicular() {
        return new Vector2D(-Y, X);
    }

    public Vector2D Clone() {
        return new Vector2D(X, Y);
    }

    public override string ToString() {
        return $"({X},{Y})";
    }

    public string ToStringRounded() {
        return $"({X:0.00},{Y:0.00})";
    }

    public float Distance(Vector2D v1) {
        var xDistance = v1.X - X;
        var yDistance = v1.Y - Y;

        return MathF.Sqrt(xDistance * xDistance + yDistance * yDistance);
    }

    public override bool Equals(object? obj) {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Vector2D)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(X, Y);
    }
}