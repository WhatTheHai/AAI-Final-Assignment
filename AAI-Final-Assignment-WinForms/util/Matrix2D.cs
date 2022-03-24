// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
// namespace AAI_Final_Assignment_WinForms.util
// {
//     public class Matrix2D
//     {
//         double[,] mat = new double[3, 3];
//
//         public Matrix2D(
//             double m11, double m12, double m13,
//             double m21, double m22, double m23,
//             double m31, double m32, double m33
//         )
//         {
//             mat[0, 0] = m11;
//             mat[0, 1] = m12;
//             mat[0, 2] = m13;
//             mat[1, 0] = m21;
//             mat[1, 1] = m22;
//             mat[1, 2] = m23;
//             mat[2, 0] = m31;
//             mat[2, 1] = m32;
//             mat[2, 2] = m33;
//         }
//
//         public Matrix2D() : this(
//             0.0, 0.0, 0.0,
//             0.0, 0.0, 0.0,
//             0.0, 0.0, 0.0)
//         {
//         }
//
//         public static Vector2D PointToLocalSpace(Vector2D point, Vector2D heading, Vector2D side, Vector2D position)
//         {
//             Vector2D TransPoint = point.Clone();
//
//
//             double Tx = -position.Dot(heading);
//             double Ty = -position.Dot(side);
//
//             Matrix2D transforMatrix = new Matrix2D(
//                 heading.X, side.X, 0,
//                 heading.Y, side.Y, 0,
//                 Tx, Ty, 1
//             );
//
//             return transforMatrix.TransformVector2D(TransPoint);
//         }
//
//         public Vector2D VectorToWorldSpace(Vector2D vector, Vector2D Heading, Vector2D Side)
//         {
//             Vector2D TransPoint = vector.Clone();
//             Matrix2D matTransform;
//
//             return null;
//         }
//
//         public void Rotate()
//         {
//             
//         }
//
//         public Vector2D TransformVector2D(Vector2D point)
//         {
//             double tempX = (mat[0, 0] * point.X) + (mat[1, 0] * point.Y) + mat[2, 0];
//             double tempY = (mat[0, 1] * point.X) + (mat[1, 1] * point.Y) + mat[2, 1];
//
//             return new Vector2D(tempX, tempY);
//         }
//
//
//     }
// }