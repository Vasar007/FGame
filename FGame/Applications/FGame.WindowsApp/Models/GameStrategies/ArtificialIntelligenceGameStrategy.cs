using System;
using FGame.DomainLogic;
using FGame.WindowsApp.Domain;
using FGame.WindowsApp.ViewModels;

namespace FGame.WindowsApp.Models.GameStrategies
{
    internal sealed class ArtificialIntelligenceGameStrategy
    {
        private readonly Random _random;


        public ArtificialIntelligenceGameStrategy(Random random)
        {
            _random = random.ThrowIfNull(nameof(random));
        }

        public Simulator.GameState Move(Simulator.GameState currentState, BrainInfoViewModel brain)
        {
            currentState.ThrowIfNull(nameof(currentState));
            brain.ThrowIfNull(nameof(brain));

            return Simulator.handleChromosomeMove(currentState, _random, brain.Model);
        }

        public Simulator.GameState Reset(Simulator.GameState? currentState)
        {
            return CreateDefaultState();
        }

        private static Simulator.GameState CreateDefaultState()
        {
            World.World world = WorldGeneration.makeDefaultWorld();
            const int turnsLeft = 30;
            return new Simulator.GameState(world, Simulator.SimulationState.Simulating, turnsLeft);
        }
    }
}
