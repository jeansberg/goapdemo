using System;

namespace Core.GameObject
{
    public struct RgbColor
    {
        public int R;
        public int G;
        public int B;

        public RgbColor(int red, int green, int blue)
        {
            R = red;
            G = green;
            B = blue;
        }

        public static RgbColor DarkGrey()
        {
            return new RgbColor(100, 100, 100);
        }

        internal static RgbColor Red()
        {
            return new RgbColor(255, 100, 100);
        }
    }
}
