using Console = SadConsole.Console;
using Core.GameObject;
using Core.Map;
using Goap;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Goap.AgentState;
using Core;
using System.Linq;
using Core.AI;

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
        private Map map;
        private Controller controller;
        private CreatureFactory _creatureFactory;
        private Renderer _renderer;

        public Game(int width, int height, CreatureFactory creatureFactory)
        {
            _width = 80;
            _height = 25;
            _creatureFactory = creatureFactory;
        }

        public void Start()
        {
            SadConsole.Game.Create("IBM.font", _width, _height);
            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.Instance.Run();

            SadConsole.Game.Instance.Dispose();
        }

        private void Init()
        {
            map = new Map(_width, _height);
            player = _creatureFactory.CreatePlayer(map, new Core.GameObject.Point(5, 5));

            var worldState = new WorldState();

            agentMaps = new Dictionary<Creature, IAgent>();
            creatures = new List<Creature>
            {
                _creatureFactory.CreateMonster(map, new Core.GameObject.Point(10, 10), GetAgent(), new List<Creature>{player }, worldState, agentMaps),
                _creatureFactory.CreateNpc(map, new Core.GameObject.Point(15, 10))
            };

            map.AddCreatures(creatures);

            startingConsole = new Console(_width, _height);
            SadConsole.Global.CurrentScreen = startingConsole;
            _renderer = new Renderer(startingConsole);
            controller = new Controller();
        }

        private void Update(GameTime time)
        {
            DrawMap(startingConsole, map);
            DrawCreatures(startingConsole, creatures.Select(c => c.MapComponent).ToList(), player);

            if (SadConsole.Global.KeyboardState.KeysReleased.Count > 0)
            {
                controller.HandleInput(SadConsole.Global.KeyboardState.KeysReleased, player);

                creatures.RemoveAll(x => !x.IsAlive());

                UpdateAI();
            }

            if (!player.IsAlive())
            {
                SadConsole.Game.Instance.Exit();
            }
        }

        private void UpdateAI()
        {
            agentMaps = agentMaps.Where(x => creatures.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

            foreach(var agent in agentMaps.Values)
            {
                agent.Update();
            }
        }

        private void DrawCreatures(Console startingConsole, List<MapComponent> creatures, Creature player)
        {
            var allCreatures = creatures;
            allCreatures.Add(player.MapComponent);
            foreach (var creature in allCreatures)
            {
                _renderer.Draw(creature);
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
                    _renderer.Draw(graphic, new Core.GameObject.Point(x, y));
                }
            }
        }
    }
}