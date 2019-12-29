using System;
using System.Collections.Generic;
using System.Linq;
using FGame.DomainLogic;
using FGame.Models;
using FGame.WindowsApp.Domain;
using FGame.WindowsApp.ViewModels;

namespace FGame.WindowsApp.Models.GameStrategies
{
    internal sealed class ArtificialIntelligenceGameStrategy
    {
        private readonly Random _random;

        private IReadOnlyList<World.World> _worlds;


        public ArtificialIntelligenceGameStrategy(Random random)
        {
            _random = random.ThrowIfNull(nameof(random));
            _worlds = GenerateWorlds(random);
        }

        public IReadOnlyList<GeneticModels.SimulationResult> AdvanceToNextGeneration(
            IEnumerable<SimulationResultViewModel> currentPopulation)
        {
            currentPopulation.ThrowIfNull(nameof(currentPopulation));

            var priorResults = currentPopulation.Select(p => p.Model);

            var brains = Population.mutateBrains(_random, priorResults.Select(r => r.Brain));

            return Population.simulateGeneration(_worlds, brains).ToList();
        }

        public IReadOnlyList<GeneticModels.SimulationResult> AdvanceToNext10Generation(
            IEnumerable<SimulationResultViewModel> currentPopulation)
        {
            currentPopulation.ThrowIfNull(nameof(currentPopulation));

            var priorResults = currentPopulation.Select(p => p.Model);

            return Population.mutateAndSimulateMultiple(_random, _worlds, 10, priorResults).ToList();
        }

        public IReadOnlyList<GeneticModels.SimulationResult> RandomizeBrains()
        {
            return Population.simulateFirstGeneration(_worlds, _random).ToList();
        }

        public IReadOnlyList<GeneticModels.SimulationResult> RandomizeWorlds(
            IEnumerable<SimulationResultViewModel> currentPopulation)
        {
            currentPopulation.ThrowIfNull(nameof(currentPopulation));

            _worlds = GenerateWorlds(_random);
            return SimulateCurrentPopulation(currentPopulation);
        }

        private IReadOnlyList<GeneticModels.SimulationResult> SimulateCurrentPopulation(
            IEnumerable<SimulationResultViewModel> currentPopulation)
        {
            if (!currentPopulation.Any()) return Array.Empty<GeneticModels.SimulationResult>();

            var pop = currentPopulation.Select(p => p.Brain.Model);

            return Population.simulateGeneration(_worlds, pop).ToList();
        }

        private static IReadOnlyList<World.World> GenerateWorlds(Random random)
        {
            return WorldGeneration.makeWorlds(random, 10);
        }
    }
}
