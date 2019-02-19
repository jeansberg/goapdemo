using Console = SadConsole.Console;
using Point = Core.GameObjects.Point;
using Core.GameObjects;
using Core.Map;
using Goap;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Goap.AgentState;
using Core;
using System.Linq;
using Demo.Fov;
using Core.AI.Goals;
using Demo.Consoles;
using SadConsole;
using System;
using Core.AI;

namespace Demo
{
    public class Game
    {
        private readonly int _width;
        private readonly int _height;
        private Console _mapConsole;
        private Console _logConsole;
        private Console _inventoryConsole;
        private List<Creature> creatures;
        private Creature player;
        private Dictionary<Creature, IAgent> agentMaps;
        private Map _map;
        private Controller controller;
        private CreatureFactory _creatureFactory;
        private ItemFactory _itemFactory;
        private IRenderer _renderer;
        IFovCalculator _fov;
        IPathFinder sharedPathFinder;

        public Game(int width, int height, CreatureFactory creatureFactory, ItemFactory itemFactory, IFovCalculator fov, IRenderer renderer)
        {
            _width = 80;
            _height = 25;
            _creatureFactory = creatureFactory;
            _itemFactory = itemFactory;
            _fov = fov;
            _renderer = renderer;
        }

        public void Start()
        {
            SadConsole.Game.Create("Cheepicus12.font", _width + 2, _height + 15);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();

            SadConsole.Game.Instance.Dispose();
        }

        private void Init()
        {
            _mapConsole = new WorldConsole { Position = new Microsoft.Xna.Framework.Point(1, 1) };
            _logConsole = new LogConsole() { Position = new Microsoft.Xna.Framework.Point(1, 29) };
            _inventoryConsole = new InventoryConsole() { Position = new Microsoft.Xna.Framework.Point(41, 29) };
            var consoleLogger = new SadConsoleLogger(_logConsole);

            _map = new Map(_width, _height);
            _map.Items = new List<MapItem> {
                _itemFactory.CreateBow(new Point(12, 12)),
                _itemFactory.CreateSword(new Point(20, 12)),
                _itemFactory.CreateLoot(new Point(20, 14)),
                _itemFactory.CreateLoot(new Point(18, 16)),
                _itemFactory.CreateLoot(new Point(20, 16)),
                _itemFactory.CreateLoot(new Point(22, 17)),
                _itemFactory.CreateLoot(new Point(18, 18)),
                _itemFactory.CreateLoot(new Point(22, 20)),};

            player = _creatureFactory.CreatePlayer(_map, new Point(25, 16));

            var worldState = new WorldState();

            agentMaps = new Dictionary<Creature, IAgent>();
            creatures = new List<Creature>
            {
                _creatureFactory.CreateMonster(_map, new Point(10, 10), GetAgent(consoleLogger), new List<Creature>{player }, worldState, agentMaps),
                //_creatureFactory.CreateNpc(_map, new Point(15, 10), GetAgent(consoleLogger), agentMaps, worldState),
                //_creatureFactory.CreateNpc(_map, new Point(25, 10), GetAgent(consoleLogger), agentMaps, worldState)
            };

            _map.Creatures = creatures;

            Global.CurrentScreen.Children.Add(_mapConsole);
            Global.CurrentScreen.Children.Add(_logConsole);
            Global.CurrentScreen.Children.Add(_inventoryConsole);
            ((Renderer)_renderer).Init(_mapConsole);
            controller = new Controller();

            sharedPathFinder = new DebugPathFinder(new FloodSpill.FloodSpiller(), _renderer);
        }

        private void Update(GameTime time)
        {
            player.Fov = _fov.GetVisibleCells(player.MapComponent.GetPosition(), _map, _renderer);
            //_map.Draw(_renderer);
            //DrawFov(player.Fov);
            DrawItems(_mapConsole, _map.Items, player);
            DrawCreatures(_mapConsole, creatures, player);
            DrawInventory();

            if (Global.KeyboardState.KeysReleased.Count > 0)
            {
                controller.HandleInput(Global.KeyboardState.KeysReleased, player, _map, 
                    () => { sharedPathFinder.PathFind(new Point(1, 1), player.MapComponent.GetPosition(), _map); UpdateAI(); creatures.RemoveAll(x => !x.IsAlive()); });
            }

            DrawTargets();

            if (!player.IsAlive())
            {
                SadConsole.Game.Instance.Exit();
            }
        }

        private void DrawInventory()
        {
            foreach(var item in player.Inventory)
            {
                _inventoryConsole.Clear(new Rectangle(0, 1, _inventoryConsole.Width, _inventoryConsole.Height - 1));
                _inventoryConsole.Cursor.Print(item.ToString());
                _inventoryConsole.Cursor.NewLine();
            }

            _inventoryConsole.Cursor.Row = 1;
        }

        private void DrawItems(Console mapConsole, List<MapItem> items, Creature player)
        {
            foreach (var item in items)
            {
                if (player.Fov.Contains(item.MapComponent.GetPosition()))
                {
                    var position = item.MapComponent.GetPosition();
                    _renderer.Draw(item.GraphicsComponent, position);
                }
            }
        }

        private void DrawTargets()
        {
            foreach(var target in controller.Targets)
            {
                var position = target.Value.MapComponent.GetPosition();
                _renderer.ShowTarget(position.XPos, position.YPos, target.Key);
            }
        }

        private void DrawFov(List<Point> fov)
        {
            foreach(var point in fov)
            {
                _renderer.LightUp(point.XPos, point.YPos);
            }
        }

        private void UpdateAI()
        {
            agentMaps = agentMaps.Where(x => creatures.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

            foreach(var mapping in agentMaps)
            {
                // Update world state with "visible creatures"
                var agent = mapping.Value;
                var creature = mapping.Key;

                var fov = _fov.GetVisibleCells(creature.MapComponent.GetPosition(), _map, _renderer);
                creature.Fov = fov;
                var objectsInFov = new List<GameObject>();
                objectsInFov.AddRange(GetAllCreatures().Where(x => fov.Contains(x.MapComponent.GetPosition()) && x != creature));
                objectsInFov.AddRange(_map.Items.Where(x => fov.Contains(x.MapComponent.GetPosition())));

                if (objectsInFov.Count > 0)
                {
                    var visibleConditionsRemoved = agent.GetWorldState().Conditions.Where(x => x.Key.GetType() != typeof(CanSeeTarget)).ToDictionary(x => x.Key, x => x.Value);
                    var newState = new WorldState(visibleConditionsRemoved);

                    foreach (var c in objectsInFov)
                    {
                        newState.Conditions.Add(new CanSeeTarget(c), true);
                    }

                    agent.UpdateWorldState(newState);
                }

                // Run GOAP stuff
                agent.Update();
            }
        }

        private void DrawCreatures(Console startingConsole, List<Creature> creatures, Creature player)
        {
            var allCreatures = new List<Creature>(creatures);
            allCreatures.Add(player);
            foreach (var creature in allCreatures)
            {
                if (player.Fov.Contains(creature.MapComponent.GetPosition()))
                {
                    creature.Draw(_renderer);
                }
            }
        }

        private IAgent GetAgent(ILogger logger)
        {
            var fsm = new AgentStateMachine();
            var planner = new GoapPlanner();

            return new GoapAgent(fsm, planner, logger);
        }

        private List<Creature> GetAllCreatures()
        {
            var all = new List<Creature>(creatures);
            all.Add(player);
            return all;
        }
    }
}