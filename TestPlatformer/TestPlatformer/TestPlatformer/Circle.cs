using System;

using Microsoft.Xna.Framework;

namespace TestPlatformer //CHANGE!
{
    /// <summary>
    /// Represents a Two Dimensional Circle.
    /// </summary>
    public struct Circle
    {
        /// <summary>
        /// The center of the circle.
        /// </summary>
        public Vector2 center;
        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public float radius;

        /// <summary>
        /// Creates a new Circle that is 'radius' wide,
        /// and is centered at 'center'.
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /// <summary>
        /// Checks if the Circle intersects with a Rectangle.
        /// (Microsoft.Xna.Framework.Rectangle)
        /// </summary>
        /// <param name="other">
        /// Will be checked for intersections with
        /// this instance of Circle.
        /// (Microsoft.Xna.Framework.Rectangle not the other kind)
        /// </param>
        /// <returns>
        /// true if the two shapes intersect anywhere.
        /// false otherwise.
        /// </returns>
        public bool intersects(Rectangle other)
        {
            if (other.Contains((int)center.X, (int)center.Y)) return true;

            Vector2 topLeft = new Vector2(other.Left, other.Top);
            Vector2 topRight = new Vector2(other.Right, other.Top);
            Vector2 bottomRight = new Vector2(other.Right, other.Bottom);
            Vector2 bottomLeft = new Vector2(other.Left, other.Bottom);

            Vector2 clamped = Vector2.Clamp(center, topLeft, topRight);
            if ((clamped - center).Length() <= radius) return true;
            clamped = Vector2.Clamp(center, topRight, bottomRight);
            if ((clamped - center).Length() <= radius) return true;
            clamped = Vector2.Clamp(center, bottomLeft, bottomRight);
            if ((clamped - center).Length() <= radius) return true;
            clamped = Vector2.Clamp(center, topLeft, bottomLeft);
            if ((clamped - center).Length() <= radius) return true;
            
            return false;
        }

        /// <summary>
        /// Overloaded '+' operator for use with instances of this Circle
        /// struct, and instances of Microsoft.Xna.Framework.Vector2 structs.
        /// 
        /// Adds 'v' to the center of 'circle'.
        /// </summary>
        /// <param name="circle">Circle instance</param>
        /// <param name="v">Vector2 instance</param>
        /// <returns>
        /// The original Circle, 'circle'.
        /// </returns>
        public static Circle operator + (Circle circle, Vector2 v)
        {
            circle.center += v;
            return circle;
        }

        /// <summary>
        /// Overloaded '-' operator for use with instances of this Circle
        /// struct, and instances of Microsoft.Xna.Framework.Vector2 structs.
        /// 
        /// Subtracts 'v' from the center of 'circle'.
        /// </summary>
        /// <param name="circle">Circle instance</param>
        /// <param name="v">Vector2 instance</param>
        /// <returns>
        /// The original Circle, 'circle'.
        /// </returns>
        public static Circle operator -(Circle circle, Vector2 v)
        {
            circle.center -= v;
            return circle;
        }
    }
}