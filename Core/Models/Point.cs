using System;
using System.Collections.Generic;

namespace Core.GameObjects
{
    public struct Point
    {
        public static Point Zero = new Point(0, 0);

        public Point(int xPos, int yPos)
        {
            this.XPos = xPos;
            this.YPos = yPos;
        }

        public int XPos { get; set; }
        public int YPos { get; set; }

        public bool IsAdjacentTo(Point otherPoint)
        {
            return IsWithin(1, otherPoint);
        }

        public List<Point> GetAdjacent()
        {
            return new List<Point>
            {
                new Point(XPos+1, YPos),
                new Point(XPos-1, YPos),
                new Point(XPos, YPos+1),
                new Point(XPos, YPos-1),

            };
        }

        public bool IsWithin(int distance, Point otherPoint)
        {
            var xDistance = Math.Abs(XPos - otherPoint.XPos);
            var yDistance = Math.Abs(YPos - otherPoint.YPos);

            return xDistance + yDistance <= distance;
        }

        public override string ToString()
        {
            return $"{XPos}, {YPos}";
        }
    }
}