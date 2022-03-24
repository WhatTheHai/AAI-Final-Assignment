using System.Numerics;
using System.Runtime.Intrinsics;

namespace AAI_Final_Assignment_WinForms.util
{

    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D() : this(0, 0)
        {
        }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public double LengthSquared()
        {
            return (X*X) + (Y*Y);
        }


        public Vector2D Add(Vector2D v)
        {
            this.X += v.X;
            this.Y += v.Y;
            return this;
        }

        public Vector2D Sub(Vector2D v)
        {
            this.X -= v.X;
            this.Y -= v.Y;
            return this;
        }

        public Vector2D Multiply(double value)
        {
            this.X *= value;
            this.Y *= value;
            return this;
        }

        public Vector2D Divide(double value) {
            if (value <= 0) {
                throw new ArgumentOutOfRangeException(nameof(value), $@"Value cannot be 0 or lower, value is {value}");
            }

            this.X /= value;
            this.Y /= value;
            return this;
        }

        // todo: remove? 
        public Double Dot(Vector2D v2)
        {
            this.X *= v2.X;
            this.Y *= v2.Y;
            return X + Y;
        }

        public Vector2D Normalize()
        {
            double currentLength = Length();
            this.X /= currentLength;
            this.Y /= currentLength;
            return this;
        }

        public Vector2D Truncate(double max)
        {
            if (Length() > max)
            {
                Normalize();
                Multiply(max);
            }
            return this;
        }

        public Vector2D Perpendicular()
        {
            return new Vector2D(-Y, X);
        }

        public Vector2D Clone()
        {
            return new Vector2D(this.X, this.Y);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public double Distance(Vector2D v1) {
            double xDistance = v1.X - this.X;
            double yDistance = v1.Y - this.Y;

            return Math.Sqrt((xDistance * xDistance) + (yDistance * yDistance));
        }
    }


}
