using Console = SadConsole.Console;
using Core.GameObject;
using Core.Map;
using Goap;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Goap.AgentState;
using Core;
using Goap.Actions;

namespace Demo
{
    public class Game
    {
        const int Width = 80;
        const int Height = 25;
        Console startingConsole;
        List<Creature> creatures;
        Creature player;
        Dictionary<Creature, IGoapAgent> agentMaps;
        Map map;
        Controller controller;

        public Game()
        {
        }

        public void Start()
        {
            SadConsole.Game.Create("IBM.font", Width, Height);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();

            SadConsole.Game.Instance.Dispose();
        }

        private void Init()
        {
            // Any custom loading and prep. We will use a sample console for now
            map = new Map(Width, Height);
            creatures = GetCreatures();
            map.AddCreatures(creatures);
            player = GetPlayer();

            AddAgents(creatures, player);

            startingConsole = new Console(Width, Height);
            SadConsole.Global.CurrentScreen = startingConsole;
            controller = new Controller();
        }

        private void AddAgents(List<Creature> creatures, Creature player)
        {
            foreach(var creature in creatures)
            {
                var action = new AttackTargetMelee(creature, player);
                var goal = new WorldState()
                {
                    Conditions = new Dictionary<string, bool>
                {
                    { "targetDamaged", true }
                }
                };

                creature.Actions.Add(action);
                creature.Goals.Add(goal);

                var agent = GetAgent();
                agent.Start(creature, new WorldState());
                agentMaps = new Dictionary<Creature, IGoapAgent>();
                agentMaps.Add(creature, agent);
            }
        }

        private void Update(GameTime time)
        {
            DrawMap(startingConsole, map);
            DrawCreatures(startingConsole, creatures, player);

            if (SadConsole.Global.KeyboardState.KeysReleased.Count > 0)
            {
                controller.HandleInput(SadConsole.Global.KeyboardState.KeysReleased, player);
                UpdateAI(creatures);
            }

            if (!player.IsAlive())
            {
                SadConsole.Game.Instance.Exit();
            }
        }

        private void UpdateAI(List<Creature> creatures)
        {
            List<Creature> inactiveCreatures = new List<Creature>();
            foreach(var agent in agentMaps.Values)
            {
                var creature = agent.GetOwner();
                if (creature.IsAlive())
                {
                    agent.Update();
                }
                else
                {
                    inactiveCreatures.Add(creature);
                }
            }

            foreach(var creature in inactiveCreatures)
            {
                agentMaps.Remove(creature);
                creatures.Remove(creature);
            }
        }

        private static void DrawCreatures(Console startingConsole, List<Creature> creatures, Creature player)
        {
            foreach (var creature in creatures)
            {
                startingConsole.SetGlyph(creature.MapComponent.Position.xPos, creature.MapComponent.Position.yPos, 'C');
            }
            startingConsole.SetGlyph(player.MapComponent.Position.xPos, player.MapComponent.Position.yPos, 'C');

        }

        private IGoapAgent GetAgent()
        {
            var fsm = new AgentStateMachine();
            var planner = new GoapPlanner();

            return new GoapAgent(fsm, planner);
        }

        private Creature GetPlayer()
        {
            var mapComponent = new MapLocation
            {
                Position = new Core.GameObject.Point(10, 5)
            };

            var player = new Creature(new List<IAction>(), new List<WorldState>(), mapComponent, map, 15);
            return player;
        }

        private List<Creature> GetCreatures()
        {
            var mapComponent2 = new MapLocation
            {
                Position = new Core.GameObject.Point(10, 10)
            };
            var creature = new Creature(mapComponent2, map, Pathfinder.GetInstance());

            return new List<Creature> { creature };
        }

        private void DrawMap(Console startingConsole, Map map)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (map.Tiles[x][y].Type == TileType.Floor)
                    {
                        startingConsole.SetGlyph(x, y, '.', Color.White);
                    }
                    else if (map.Tiles[x][y].Type == TileType.Wall)
                    {
                        startingConsole.SetGlyph(x, y, 'x', Color.White);
                    }
                    else if (map.Tiles[x][y].Type == TileType.Debug)
                    {
                        startingConsole.SetGlyph(x, y, 'o', Color.Red);
                    }
                }
            }
        }
    }
}