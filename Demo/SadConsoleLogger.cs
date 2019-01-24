using Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Console = SadConsole.Console;

namespace Demo
{
    public class SadConsoleLogger : ILogger
    {
        private Console _console;
        private List<string> _messages;

        public SadConsoleLogger(Console console)
        {
            _console = console;
            _messages = new List<string>();
            _console.Cursor.Down(1);
        }

        public void Log(string message)
        {
            _messages.Add(message);

            _console.Clear(new Rectangle(0, 1, _console.Width, _console.Height-1));
 
            for(var i = _messages.Count - 1; i >= 0; i--)
            {
                _console.Cursor.Print(_messages[i]);
                _console.Cursor.NewLine();
            }

            _console.Cursor.Row = 1;

            if(_messages.Count == 8)
            {
                _messages.RemoveAt(0);
            }
        }
    }
}
