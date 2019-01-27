using Console = SadConsole.Console;
using System;
using Microsoft.Xna.Framework;
using SadConsole;

namespace Demo.Consoles
{
    public class TitleConsole : Console
    {
        Console borderSurface;

        public TitleConsole(string title, int width, int height) : base(width, height)
        {
            Print(0, 0, title.Align(HorizontalAlignment.Center, Width), Color.Black, Color.Yellow);

            borderSurface = new Console(width + 2, height + 2, Font);
            borderSurface.DrawBox(new Rectangle(0, 0, borderSurface.Width, borderSurface.Height),
                new Cell(Color.White, Color.Black), null, ConnectedLineThin);


            borderSurface.Position = new Point(-1, -1);

            Children.Add(borderSurface);
        }
    }
}
