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
        Dictionary<Creature, IGoapAgent> agentMaps;
        Map map;        

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

            startingConsole = new Console(Width, Height);
            SadConsole.Global.CurrentScreen = startingConsole;
        }

        private void Update(GameTime time)
        {
            if (SadConsole.Global.KeyboardState.IsKeyReleased(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                UpdateCreatures(creatures);

                DrawMap(startingConsole, map);
                DrawCreatures(startingConsole, creatures);
            }
        }

        private void UpdateCreatures(List<Creature> creatures)
        {
            foreach(var agent in agentMaps.Values)
            {
                agent.Update();
            }
        }

        private static void DrawCreatures(Console startingConsole, List<Creature> creatures)
        {
            foreach (var creature in creatures)
            {
                startingConsole.SetGlyph(creature.MapComponent.Position.xPos, creature.MapComponent.Position.yPos, 'C');
            }
        }

        private IGoapAgent GetAgent()
        {
            var fsm = new AgentStateMachine();
            var planner = new GoapPlanner();

            return new GoapAgent(fsm, planner);
        }

        private List<Creature> GetCreatures()
        {
            var mapComponent = new MapLocation
            {
                Position = new Core.GameObject.Point(5, 5)
            };

            var target = new Creature(new List<IAction>(), new List<WorldState>(), mapComponent, map);

            var mapComponent2 = new MapLocation
            {
                Position = new Core.GameObject.Point(10, 10)
            };
            var creature = new Creature(mapComponent2, map);

            var action = new AttackTargetMelee(creature, target);
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

            return new List<Creature> { target, creature };
        }

        private void DrawMap(Console startingConsole, Map map)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (map.Tiles[x][y].Type == TileType.Floor)
                    {
                        startingConsole.SetGlyph(x, y, '.');
                    }
                    else if (map.Tiles[x][y].Type == TileType.Wall)
                    {
                        startingConsole.SetGlyph(x, y, 'x');
                    }
                }
            }
        }
    }
}