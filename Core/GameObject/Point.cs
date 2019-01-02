using System;

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