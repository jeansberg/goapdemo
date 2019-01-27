using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.GameObject;
using Core.Map;
using Microsoft.Xna.Framework;
using SadConsole.Input;
using Point = Core.GameObject.Point;

namespace Demo
{
    enum ControllerState
    {
        Movement,
        Targeting
    }

    public class Controller
    {
        private static AsciiKey left = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Left);
        private static AsciiKey right = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Right);
        private static AsciiKey up = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Up);
        private static AsciiKey down = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.Down);
        private static AsciiKey l = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.L);
        private static AsciiKey t = AsciiKey.Get(Microsoft.Xna.Framework.Input.Keys.T);

        private ControllerState _state;

        public Controller()
        {
            _state = ControllerState.Movement;
            Targets = new Dictionary<int, Creature>();
        }

        public Dictionary<int, Creature> Targets { get; private set; }

        internal void HandleInput(List<AsciiKey> keysReleased, Creature player, Map map, Action endTurn)
        {
            switch (_state)
            {
                case ControllerState.Movement:
                    if (keysReleased.Contains(left))
                    {
                        player.MoveAttack(Direction.Left);
                    }
                    else if (keysReleased.Contains(right))
                    {
                        player.MoveAttack(Direction.Right);
                    }
                    else if (keysReleased.Contains(up))
                    {
                        player.MoveAttack(Direction.Up);
                    }
                    else if (keysReleased.Contains(down))
                    {
                        player.MoveAttack(Direction.Down);
                    }
                    else if (keysReleased.Contains(l))
                    {
                        var position = player.MapComponent.GetPosition();
                        var item = map.Items.SingleOrDefault(x => x.MapComponent.GetPosition().Equals(position));
                        if(item != null)
                        {
                            player.Inventory.Add(item.InventoryItem);
                        }

                        map.Items.Remove(item);
                    }
                    else if (keysReleased.Contains(t))
                    {
                        SetTargetingState(map, player);
                        return;
                    }
                    endTurn();

                    break;
                case ControllerState.Targeting:
                    foreach(var key in keysReleased)
                    {
                        Targets.TryGetValue(key.Character, out Creature target);
                        if(target != null)
                        {
                            player.Attack(target);
                            endTurn();
                            break;
                        }
                    }


                    Targets.Clear();
                    SetMovementState();
                    break;
            }
        }

        private void SetMovementState()
        {
            _state = ControllerState.Movement;
        }

        private void SetTargetingState(Map map, Creature player)
        {
            _state = ControllerState.Targeting;

            var creaturesInFov = map.Creatures.Where(x => player.Fov.Contains(x.MapComponent.GetPosition()) && x != player).ToList();

            foreach (var creature in creaturesInFov)
            {
                Targets.Add(Targets.Count + 97, creature);
            }
        }
    }
}
