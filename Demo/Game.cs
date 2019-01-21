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
using Core.AI.Goals;
using Demo.Consoles;

namespace Demo
{
    public class Game
    {
        private readonly int _width;
        private readonly int _height;
        private Console _mapConsole;
        private Console _logConsole;
        private List<Creature> creatures;
        private Creature player;
        private Dictionary<Creature, IAgent> agentMaps;
        private Map _map;
        private Controller controller;
        private CreatureFactory _creatureFactory;
        private Renderer _renderer;
        FovCalculator _fov;

        public Game(int width, int height, CreatureFactory creatureFactory, FovCalculator fov, Renderer renderer)
        {
            _width = 80;
            _height = 25;
            _creatureFactory = creatureFactory;
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
            var consoleLogger = new SadConsoleLogger(_logConsole);

            _map = new Map(_width, _height);
            player = _creatureFactory.CreatePlayer(_map, new Point(25, 16));

            var worldState = new WorldState();

            agentMaps = new Dictionary<Creature, IAgent>();
            creatures = new List<Creature>
            {
                _creatureFactory.CreateMonster(_map, new Point(10, 10), GetAgent(consoleLogger), new List<Creature>{player }, worldState, agentMaps),
                _creatureFactory.CreateNpc(_map, new Point(15, 10))
            };

            _map.AddCreatures(creatures);

            SadConsole.Global.CurrentScreen.Children.Add(_mapConsole);
            SadConsole.Global.CurrentScreen.Children.Add(_logConsole);
            _renderer.Init(_mapConsole);
            controller = new Controller();
        }

        private void Update(GameTime time)
        {
            player.Fov = _fov.GetVisibleCells(player.MapComponent.Position, _map, _renderer);
            _map.Draw(_renderer);
            DrawFov(player.Fov);
            DrawCreatures(_mapConsole, creatures, player);

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

        private void DrawCreatures(Console startingConsole, List<Creature> creatures, Creature player)
        {
            var allCreatures = new List<Creature>(creatures);
            allCreatures.Add(player);
            foreach (var creature in allCreatures)
            {
                if (player.Fov.Contains(creature.MapComponent.Position))
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