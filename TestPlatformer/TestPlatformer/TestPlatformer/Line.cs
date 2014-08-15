using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TestPlatformer {
    /// <summary>
    /// Represents a 2D line segment for collision detection/response purposes
    /// </summary>
    public struct Line
    {
        private Vector2 a, b;
        //a is left endpoint
        //b is right endpoint

        private float slope; //slope of the line

        private float yAt0;
        //the vertical position of the line at the origin

        /// <summary>
        /// The left endpoint of the line segment
        /// 
        /// WILL THROW A DIVIDE BY ZERO EXCEPTION IF pt1.X == pt2.X!!!!!!
        /// </summary>
        public Vector2 pt1
        {
            get
            {
                return a;
            }
            set
            {
                a = value;
                adjustForNewEndPoint();
            }
        }

        /// <summary>
        /// The right endpoint of the line segment
        /// 
        /// WILL THROW A DIVIDE BY ZERO EXCEPTION IF pt1.X == pt2.X!!!!!!
        /// </summary>
        public Vector2 pt2
        {
            get
            {
                return b;
            }
            set
            {
                b = value;
                adjustForNewEndPoint();
            }
        }

        public Rectangle getBoundingRectangle {
            get {
                Rectangle tempRectangle = new Rectangle();
                if (pt1.Y <= pt2.Y) {
                    if (pt1.X <= pt2.X) {
                        tempRectangle = new Rectangle((int)pt1.X, (int)pt1.Y, (int)(pt2.X - pt1.X), (int)(pt2.Y-pt1.Y));
                    }
                    else if(pt1.X >= pt2.X) { 
                        tempRectangle = new Rectangle((int)pt2.X, (int)pt1.Y, (int)(pt1.X - pt2.X), (int)(pt2.Y-pt1.Y));
                    }
                }
                else if (pt1.Y >= pt2.Y) {
                    if (pt1.X <= pt2.X)
                    {
                        tempRectangle = new Rectangle((int)pt1.X, (int)pt2.Y, (int)(pt2.X - pt1.X), (int)(pt1.Y - pt2.Y));
                    }
                    else if (pt1.X >= pt2.X)
                    {
                        tempRectangle = new Rectangle((int)pt2.X, (int)pt2.Y, (int)(pt1.X - pt2.X), (int)(pt1.Y - pt2.Y));
                    }
                }
                return tempRectangle;
            }
        }

        public Rectangle getAltBoundingRectangle(Vector2 altPos)
        {
            return new Rectangle((int)altPos.X, (int)altPos.Y, getBoundingRectangle.Width, getBoundingRectangle.Height);
        }
        /// <summary>
        /// Initializes a new line segment with the given coordinates
        /// 
        /// WILL THROW A DIVIDE BY ZERO EXCEPTION IF pt1.X == pt2.X!!!!!!
        /// </summary>
        /// <param name="pt1">Left endpoint of the line segment</param>
        /// <param name="pt2">Right endpoint of the line segment</param>
        public Line(Vector2 pt1, Vector2 pt2)
        {
            a = pt1;
            b = pt2;

            if (a.X > b.X) //pt1 is farther right than pt2, swap them
            {
                Vector2 tmp = a;
                a = b;
                b = tmp;
            }

            slope = (b.Y - a.Y) / (b.X - a.X);

            yAt0 = a.Y - (slope * a.X);
        }

        /// <summary>
        /// Calculates the slope and swaps a (pt1) and b (pt2) if necessary
        /// Is called whenever one of the endpoints is changed.
        /// </summary>
        private void adjustForNewEndPoint()
        {
            if (a.X > b.X)
            {
                Vector2 tmp = a;
                a = b;
                b = tmp;
            }

            slope = (b.Y - a.Y) / (b.X - a.X);

            yAt0 = a.Y - (slope * a.X);
        }

        /// <summary>
        /// Returns the slope of the line segment.
        /// </summary>
        /// <returns>Returns the slope of the line segment</returns>
        public float getSlope()
        {
            return slope;
        }

        /// <summary>
        /// Returns the y position on the line at any given x position.
        /// Will treat the line segment as an infinite line if the x value is
        /// not between the two endpoints of the line segment.
        /// </summary>
        /// <param name="x">A horizontal position</param>
        /// <returns>The lines y position at the given x position</returns>
        public float yAtX(float x)
        {
            return (x * slope) + yAt0;
        }

        /// <summary>
        /// Returns the x position on the line at any given y position.
        /// Will treat the line segment as an infinite line if the y value is
        /// not between the two endpoints of the line segment.
        /// </summary>
        /// <param name="x">A vertical position</param>
        /// <returns>The lines y position at the given x position</returns>
        public float xAtY(float y)
        {
            return (y - yAt0) / slope;
        }

        /// <summary>
        /// If this line segment lies between pos and prevPos, and pos has a
        /// greater or equal Y position than prevPos, a Rectangle
        /// will be returned based on pos's position adjusted in the Y
        /// direction so that corner of the resulting rectangle, that is closest
        /// to this line segment, lies directly on this line segment.
        /// </summary>
        /// <param name="pos">
        /// The position of a Rectangle (prevPos) after a downward or
        /// horizontal movement (pos).
        /// </param>
        /// <param name="prevPos">
        /// The position of a Rectangle (prevPos) before a downward or
        /// horizontal movement (pos).
        /// </param>
        /// <returns></returns>
        public Rectangle verticalCollisionResponse
            (Rectangle pos, Rectangle prevPos)
        {
            if (a.X == b.X)
            {
                if (prevPos.Right <= a.X && pos.Left > prevPos.Left)
                {
                    if (intersects(new Line(
                            new Vector2(prevPos.Right, prevPos.Top),
                            new Vector2(pos.Right, pos.Top))) ||
                        intersects(new Line(
                            new Vector2(prevPos.Right, prevPos.Bottom),
                            new Vector2(pos.Right, pos.Bottom))))
                    {
                        return new Rectangle((int)(a.X - pos.Width), pos.Top,
                            pos.Width, pos.Height);
                    }
                }
                if (prevPos.Right >= a.X && pos.Left < prevPos.Left)
                {
                    if (intersects(new Line(
                            new Vector2(prevPos.Left, prevPos.Top),
                            new Vector2(pos.Left, pos.Top))) ||
                        intersects(new Line(
                            new Vector2(prevPos.Left, prevPos.Bottom),
                            new Vector2(pos.Left, pos.Bottom))))
                    {
                        return new Rectangle((int)a.X, pos.Top,
                            pos.Width, pos.Height);
                    }
                }
                return pos;
            }

            if (pos.Right < a.X || pos.Left > b.X)
                return pos;//no collision occured

            if (prevPos.Top <= pos.Top)
            //only checks for collisions in downward or horizontal direction
            {
                if (slope >= 0)
                {
                    if (prevPos.Bottom <= yAtX(prevPos.Left) &&
                        (intersects(new Line(
                            new Vector2(prevPos.Left, prevPos.Bottom),
                            new Vector2(pos.Left, pos.Bottom))) ||
                        intersects(new Line(
                            new Vector2(prevPos.Right, prevPos.Bottom),
                            new Vector2(pos.Right, pos.Bottom)))))
                    //handles collisions on the Left side of the rectangle.
                    {
                        return new Rectangle(
                            pos.Left, (int)(yAtX(pos.Left) - pos.Height),
                            pos.Width, pos.Height);
                    }

                    if (prevPos.Left <= a.X && prevPos.Bottom <= a.Y &&
                        pos.Bottom > a.Y)
                    //handles collisions at the Left end of the line segment.
                    {
                        return new Rectangle(
                            pos.Left, (int)(a.Y - pos.Height),
                            pos.Width, pos.Height);
                    }

                }
                if (slope <= 0)
                {
                    if (prevPos.Bottom <= yAtX(prevPos.Right) &&
                        (intersects(new Line(
                        new Vector2(prevPos.Right, prevPos.Bottom),
                        new Vector2(pos.Right, pos.Bottom))) ||
                        intersects(new Line(
                        new Vector2(prevPos.Left, prevPos.Bottom),
                        new Vector2(pos.Left, pos.Bottom)))))
                    //handles collisions on the Right side of the Rectangle.
                    {
                        return new Rectangle(
                            pos.Left, (int)(yAtX(pos.Right) - pos.Height - 1),
                            pos.Width, pos.Height);
                    }

                    if (prevPos.Right >= b.X && prevPos.Bottom <= b.Y &&
                        pos.Bottom > b.Y)
                    //handles collisions at the Left end of the line segment.
                    {
                        return new Rectangle(
                            pos.Left, (int)(b.Y - pos.Height),
                            pos.Width, pos.Height);
                    }
                }
            }

            return pos; //if this line is reached, no collision occured.
        }

        /// <summary>
        /// Returns information about a possible intersection between
        /// this line segment and the given line segment.
        /// </summary>
        /// <param name="other">
        /// A line segment to check for intersections with this line segment.
        /// </param>
        /// <returns>
        /// Returns a CollisionInfo object with a boolean 'collisionOccured'
        /// which will be true if the two line segments intersect,
        /// and a Vector2 object representing the point of intersection
        /// between the two line segments. (will be (0, 0) if the lines do not
        /// intersect).
        /// </returns>
        public CollisionInfo getCollisionInfo(Line other)
        {
            float den = ((other.b.Y - other.a.Y) * (b.X - a.X)) -
                ((other.b.X - other.a.X) * (b.Y - a.Y));

            float num1 = ((other.b.X - other.a.X) * (a.Y - other.a.Y)) -
                ((other.b.Y - other.a.Y) * (a.X - other.a.X));

            float num2 = ((b.X - a.X) * (a.Y - other.a.Y)) -
                ((b.Y - a.Y) * (a.X - other.a.X));

            //check if the lines are parallel and NOT coincident
            if (Math.Abs(den) <= 0.00001 &&
                Math.Abs(num1) >= 0.00001f &&
                Math.Abs(num2) >= 0.00001f)
                //uses <= 0.00001 instead of == 0 for possible loss of precision.
                return new CollisionInfo(false, Vector2.Zero);

            num1 /= den;
            num2 /= den;

            //check if an intersection occured on the line segments
            if (num1 >= 0 && num1 <= 1 && num2 >= 0 && num2 <= 1)
            {
                Vector2 intsxn = new Vector2(a.X + (num1 * (b.X - a.X)),
                        a.Y + (num1 * (b.Y - a.Y)));
                return new CollisionInfo(true, intsxn);
            }
            else return new CollisionInfo(false, Vector2.Zero);
        }

        /// <summary>
        /// Checks if this Line segment the given 'other' Line segment
        /// intersect eachother.
        /// </summary>
        /// <param name="other">
        /// The line segment to check for intersections with this Line segment
        /// </param>
        /// <returns>
        /// True if the two line segments intersect.
        /// False otherwise.
        /// </returns>
        public bool intersects(Line other)
        {
            return getCollisionInfo(other).collisionOccured;
        }

        /// <summary>
        /// Convienience operator overload for Line segments.
        /// Will move the entire Line segment in the direction and length
        /// of 'v'.
        /// </summary>
        /// <param name="line">The line segment to move.</param>
        /// <param name="v">The direction and length to move 'line' in</param>
        /// <returns>The shifted line segment.</returns>
        public static Line operator +(Line line, Vector2 v)
        {
            line.pt1 += v;
            line.pt2 += v;
            return line;
        }

        /// <summary>
        /// Convienience operator overload for Line segments.
        /// Will move the entire Line segment in the opposite direction
        /// and length of 'v'.
        /// </summary>
        /// <param name="line">The line segment to move.</param>
        /// <param name="v">
        /// The opposite direction and length to move 'line' in.
        /// </param>
        /// <returns>The shifted line segment.</returns>
        public static Line operator -(Line line, Vector2 v)
        {
            line.pt1 -= v;
            line.pt2 -= v;
            return line;
        }
    }

    /// <summary>
    /// Contains information about a possible collision/intersection
    /// between two 2D shapes.
    /// </summary>
    public struct CollisionInfo
    {
        /// <summary>
        /// Is True if a collision occured.
        /// </summary>
        public bool collisionOccured;

        /// <summary>
        /// Contains the point of intersection between two objects.
        /// Should only be used if 'collisionOccured' is true.
        /// </summary>
        public Vector2 intersection;

        /// <summary>
        /// Initializes a CollisionInfo object.
        /// </summary>
        /// <param name="collisionOccured">
        /// Should be true if a collision occured.
        /// False otherwise
        /// </param>
        /// <param name="intersection">
        /// The point of intersection between two objects.
        /// </param>
        public CollisionInfo(bool collisionOccured, Vector2 intersection)
        {
            this.collisionOccured = collisionOccured;
            this.intersection = intersection;
        }

        /// <summary>
        /// Returns a String representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "CollisionInfo:\ncollisionOccured: " +
                Convert.ToString(collisionOccured) +
                "\nintersection: " + intersection.ToString();
        }
    }
}