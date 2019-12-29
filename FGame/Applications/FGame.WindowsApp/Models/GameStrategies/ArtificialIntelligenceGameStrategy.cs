using System;
using FGame.DomainLogic;
using FGame.Models;
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

        public States.GameState Move(States.GameState currentState, BrainInfoViewModel brain)
        {
            currentState.ThrowIfNull(nameof(currentState));
            brain.ThrowIfNull(nameof(brain));

            return Simulator.handleChromosomeMove(currentState, _random, brain.Model);
        }

        public States.GameState Reset(States.GameState? currentState)
        {
            return CreateDefaultState();
        }

        private static States.GameState CreateDefaultState()
        {
            World.World world = WorldGeneration.makeDefaultWorld();
            const int turnsLeft = 30;
            return new States.GameState(world, States.SimulationState.Simulating, turnsLeft);
        }
    }
}
