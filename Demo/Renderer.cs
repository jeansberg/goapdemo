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

        internal void LightUp(int x, int y)
        {
            _console.SetForeground(x, y, Color.LightGray);
        }

        internal void ShowTarget(int x, int y, int index)
        {
            _console.SetBackground(x, y, Color.DarkRed);
            _console.SetGlyph(x, y, index, Color.LightPink);
        }

        internal void ResetBackGround()
        {
            for(var x = 0; x < 80; x++)
            {
                for (var y = 0; y < 25; y++)
                {
                    _console.SetBackground(x, y, Color.Black);
                }
            }
        }

        internal void DrawLine(int x1, int y1, int x2, int y2)
        {
            _console.DrawLine(new Microsoft.Xna.Framework.Point(x1, y2), new Microsoft.Xna.Framework.Point(x2, y2), Color.Red);
        }
    }
}
