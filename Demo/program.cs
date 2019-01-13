using Core.AI;
using Core.GameObject;
using Demo.Fov;
using FloodSpill;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(80, 25, new CreatureFactory(new DebugPathFinder(new FloodSpiller())), new FovCalculator(10));
            game.Start();
        }
    }
}
