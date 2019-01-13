using Console = SadConsole.Console;
using Point = Core.GameObject.Point;
using Core.GameObject;
using Core.Map;
using Goap;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Goap.AgentState;
using Core;
using System.Linq;
using Core.AI;
using Demo.Fov;
using System;
using Goap.Actions;

namespace Demo
{
    public class Game
    {
        private readonly int _width;
        private readonly int _height;
        private Console startingConsole;
        private List<Creature> creatures;
        private Player player;
        private Dictionary<Creature, IAgent> agentMaps;
        private Map _map;
        private Controller controller;
        private CreatureFactory _creatureFactory;
        private Renderer _renderer;
        FovCalculator _fov;

        public Game(int width, int height, CreatureFactory creatureFactory, FovCalculator fov)
        {
            _width = 80;
            _height = 25;
            _creatureFactory = creatureFactory;
            _fov = fov;
        }

        public void Start()
        {
            SadConsole.Game.Create("Cheepicus12.font", _width, _height);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();

            SadConsole.Game.Instance.Dispose();
        }

        private void Init()
        {
            _map = new Map(_width, _height);
            player = _creatureFactory.CreatePlayer(_map, new Point(25, 15));

            var worldState = new WorldState();

            agentMaps = new Dictionary<Creature, IAgent>();
            creatures = new List<Creature>
            {
                _creatureFactory.CreateMonster(_map, new Point(10, 10), GetAgent(), new List<Creature>{player }, worldState, agentMaps),
                _creatureFactory.CreateNpc(_map, new Core.GameObject.Point(15, 10))
            };

            _map.AddCreatures(creatures);

            startingConsole = new Console(_width, _height);
            SadConsole.Global.CurrentScreen = startingConsole;
            _renderer = new Renderer(startingConsole);
            controller = new Controller();
        }

        private void Update(GameTime time)
        {
            player.Fov = _fov.GetVisibleCells(player.MapComponent.Position, _map, _renderer);
            DrawMap(startingConsole, _map);
            DrawFov(player.Fov);
            DrawCreatures(startingConsole, creatures.Select(c => c.MapComponent).ToList(), player);

            if (SadConsole.Global.KeyboardState.KeysReleased.Count > 0)
            {
                controller.HandleInput(SadConsole.Global.KeyboardState.KeysReleased, player, _map, _renderer, 
                    () => { UpdateAI(); creatures.RemoveAll(x => !x.IsAlive()); });
            }

            DrawTargets();

            if (!player.IsAlive())
            {
                SadConsole.Game.Instance.Exit();
            }
        }

        private void DrawTargets()
        {
            foreach(var target in controller.Targets)
            {
                var position = target.Value.MapComponent.Position;
                _renderer.ShowTarget(position.xPos, position.yPos, target.Key);
            }
        }

        private void DrawFov(List<Point> fov)
        {
            foreach(var point in fov)
            {
                _renderer.LightUp(point.xPos, point.yPos);
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

                var fov = _fov.GetVisibleCells(creature.MapComponent.Position, _map, _renderer);
                creature.Fov = fov;
                var creaturesInFov = GetAllCreatures().Where(x => fov.Contains(x.MapComponent.Position) && x != creature).ToList();

                if(creaturesInFov.Count > 0)
                {
                    var visibleConditionsRemoved = agent.GetWorldState().Conditions.Where(x => x.Key.GetType() != typeof(TargetVisibleCondition)).ToDictionary(x => x.Key, x => x.Value);
                    var newState = new WorldState(visibleConditionsRemoved);

                    foreach (var c in creaturesInFov)
                    {
                        newState.Conditions.Add(new TargetVisibleCondition(c), true);
                    }

                    agent.UpdateWorldState(newState);
                }

                // Run GOAP stuff
                agent.Update();
            }
        }

        private void DrawCreatures(Console startingConsole, List<MapComponent> creatures, Creature player)
        {
            var allCreatures = creatures;
            allCreatures.Add(player.MapComponent);
            foreach (var creature in allCreatures)
            {
                if (player.Fov.Contains(creature.Position))
                {
                    _renderer.Draw(creature);
                }
            }
        }

        private IAgent GetAgent()
        {
            var fsm = new AgentStateMachine();
            var planner = new GoapPlanner();

            return new GoapAgent(fsm, planner);
        }

        private void DrawMap(Console startingConsole, Map map)
        {
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var graphic = map.Tiles[x][y].Graphic;
                    _renderer.Draw(graphic, new Point(x, y));
                }
            }
        }

        private List<Creature> GetAllCreatures()
        {
            var all = new List<Creature>(creatures);
            all.Add(player);
            return all;
        }
    }
}