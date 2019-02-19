using Core.GameObjects;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using Point = Core.GameObjects.Point;

namespace Core
{
    public class Renderer : IRenderer
    {
        private Console _console;

        public Renderer()
        {
        }

        public void Init(Console console)
        {
            _console = console;
        }

        public void Draw(GraphicsComponent graphic, Point point)
        {
            var foreColor = graphic.ForeColor;
            var backColor = graphic.BackColor;
            var character = graphic.Character;
            _console.SetGlyph(point.XPos, point.YPos + 1, character, 
                new Color(foreColor.Red, foreColor.Green, foreColor.Blue), 
                new Color(backColor.Red, backColor.Green, backColor.Blue));
        }

        public void LightUp(int x, int y)
        {
            var old = _console.GetForeground(x, y);
            _console.SetForeground(x, y, old.FillBlue());
        }

        public void Highlight(int x, int y)
        {
            _console.SetForeground(x, y, Color.Red);
        }

        public void Highlight(int x, int y, RgbColor color)
        {
            _console.SetBackground(x, y, new Color(color.Red, color.Green, color.Blue));
        }

        public void ShowTarget(int x, int y, int index)
        {
            _console.SetBackground(x, y, Color.DarkRed);
            _console.SetGlyph(x, y, index, Color.LightPink);
        }

        public void ResetBackGround()
        {
            for(var x = 0; x < 80; x++)
            {
                for (var y = 0; y < 25; y++)
                {
                    _console.SetBackground(x, y, Color.Black);
                }
            }
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            _console.DrawLine(new Microsoft.Xna.Framework.Point(x1, y2), new Microsoft.Xna.Framework.Point(x2, y2), Color.Red);
        }
    }
}
