using System;
using System.Collections.Generic;

namespace Core.GameObject
{
    public class Point
    {
        public Point(int xPos, int yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;
        }

        public int xPos { get; set; }
        public int yPos { get; set; }

        public bool IsAdjacentTo(Point otherPoint)
        {
            return IsWithin(1, otherPoint);
        }

        public List<Point> GetAdjacent()
        {
            return new List<Point>
            {
                new Point(xPos+1, yPos),
                new Point(xPos-1, yPos),
                new Point(xPos, yPos+1),
                new Point(xPos, yPos-1),

            };
        }

        public bool IsWithin(int distance, Point otherPoint)
        {
            var xDistance = Math.Abs(xPos - otherPoint.xPos);
            var yDistance = Math.Abs(yPos - otherPoint.yPos);

            return xDistance + yDistance == distance;
        }

        public override string ToString()
        {
            return $"{xPos}, {yPos}";
        }
    }
}