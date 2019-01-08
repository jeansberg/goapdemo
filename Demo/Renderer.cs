using Core.GameObject;
using Microsoft.Xna.Framework;
using System;
using Console = SadConsole.Console;
using Point = Core.GameObject.Point;

namespace Core
{
    public class Renderer
    {
        private readonly Console _console;

        public Renderer(Console console)
        {
            _console = console;
        }

        public void Draw(Graphic graphic, Point point)
        {
            var foreColor = graphic.ForeColor;
            var backColor = graphic.BackColor;
            var character = graphic.Character;
            _console.SetGlyph(point.xPos, point.yPos, character, 
                new Color(foreColor.Red, foreColor.Green, foreColor.Blue), 
                new Color(backColor.Red, backColor.Green, backColor.Blue));
        }

        public void Draw(MapComponent mapComponent)
        {
            Draw(mapComponent.Graphic, mapComponent.Position);
        }
    }
}
