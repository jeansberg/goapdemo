using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.GameObject;
using SadConsole.Input;

namespace Demo
{
    public class Controller
    {
        private AsciiKey left = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Left);
        private AsciiKey right = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Right);
        private AsciiKey up = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Up);
        private AsciiKey down = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Down);

        internal void HandleInput(List<AsciiKey> keysReleased, Creature player)
        {
            if (keysReleased.Contains(left))
            {
                player.Move(Direction.Left);
            }
            else if (keysReleased.Contains(right))
            {
                player.Move(Direction.Right);
            }
            else if (keysReleased.Contains(up))
            {
                player.Move(Direction.Up);
            }
            else if (keysReleased.Contains(down))
            {
                player.Move(Direction.Down);
            }
        }
    }
}
