using Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Console = SadConsole.Console;

namespace Demo
{
    public class SadConsoleLogger : ILogger
    {
        private Console _console;
        private List<string> _messageQueue;

        public SadConsoleLogger(Console console)
        {
            _console = console;
            _messageQueue = new List<string>();
            _console.Cursor.Down(1);
        }

        public void Log(string message)
        {
            _messageQueue.Add(message);

            _console.Clear(new Rectangle(0, 1, _console.Width, _console.Height-1));

            var q = new Queue<string>(_messageQueue);
 
            while(q.Count > 0)
            {
                _console.Cursor.Print(q.Dequeue());
                _console.Cursor.NewLine();
            }

            _console.Cursor.Row = 1;
        }
    }
}
