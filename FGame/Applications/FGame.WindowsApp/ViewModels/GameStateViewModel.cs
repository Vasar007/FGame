using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FGame.Models;
using FGame.WindowsApp.Domain;
using Prism.Mvvm;

namespace FGame.WindowsApp.ViewModels
{
    /// <summary>
    /// Represents a single state in a single squirrel's simulation run.
    /// </summary>
    internal sealed class GameStateViewModel : BindableBase
    {
        public States.GameState State { get; }

        public ICollection<ActorViewModel> Actors { get; }

        public ICollection<HeatMapViewModel> HeatMap { get; }

        public string GameStatusText => State.SimState switch
        {
            States.SimulationState.Won  => "Won",

            States.SimulationState.Lost => "Lost",

            _                           => "Simulating"
        };

        public Brush GameStatusBrush => State.SimState switch
        {
            States.SimulationState.Won  => Brushes.MediumSeaGreen,

            States.SimulationState.Lost => Brushes.LightCoral,

            _                           => Brushes.LightGray
        };

        public string TurnsLeftText => State.TurnsLeft == 1
            ? "1 Turn Left"
            : $"{State.TurnsLeft.ToString()} Turns Left";


        public GameStateViewModel(States.GameState state, GeneticModels.ActorChromosome brain)
        {
            State = state.ThrowIfNull(nameof(state));
            brain.ThrowIfNull(nameof(brain));

            Actors = CreateActors(state);
            HeatMap = CreateHeatMap(state, brain);
        }

        private static List<ActorViewModel> CreateActors(States.GameState state)
        {
            return state.World.Actors
                .Where(a => a.IsActive)
                .Select(actor => new ActorViewModel(actor))
                .ToList();
        }

        private static List<HeatMapViewModel> CreateHeatMap(States.GameState state,
            GeneticModels.ActorChromosome brain)
        {
            var values = new Dictionary<WorldPos.WorldPos, double>();

            for (int y = 1; y <= state.World.MaxY; ++y)
            {
                for (int x = 1; x <= state.World.MaxX; ++x)
                {
                    var pos = new WorldPos.WorldPos(x, y);
                    values[pos] = GeneticModels.evaluateTile(brain, state.World, pos);
                }
            }

            double min = values.Values.Min();
            double max = values.Values.Max();

            return values
                .Select(kvPair => new HeatMapViewModel(kvPair.Key, kvPair.Value, min, max))
                .ToList();
        }
    }
}
