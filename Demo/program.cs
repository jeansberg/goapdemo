using Core;
using Core.AI;
using Core.GameObjects;
using Demo.Fov;
using FloodSpill;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var renderer = new Renderer();

            var game = new Game(80, 25, new CreatureFactory(new DebugPathFinder(new FloodSpiller(), renderer)), new ItemFactory(), new ShadowCastFovCalculator(10), renderer);
            game.Start();
        }
    }
}
