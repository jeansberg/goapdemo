using System;

namespace Core.GameObjects
{
    public struct RgbColor
    {
        public int Red;
        public int Green;
        public int Blue;

        public RgbColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static RgbColor CreateDarkGrey()
        {
            return new RgbColor(100, 100, 100);
        }

        public static RgbColor CreateRed()
        {
            return new RgbColor(255, 100, 100);
        }
    }
}
