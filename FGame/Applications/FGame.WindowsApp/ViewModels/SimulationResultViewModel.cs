using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using FGame.Models;
using FGame.WindowsApp.Domain;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class SimulationResultViewModel : BindableBase
    {
        private readonly ObservableCollection<GameStateViewModel> _states;

        private int _currentIndex;

        public double Score => Model.TotalScore;
        
        public string ScoreText => $"Score: {Score:F1}";

        public int Age => Model.Brain.Age;

        public string DisplayText => $"Gen {Age.ToString()} - {ScoreText}";

        public IEnumerable<GameStateViewModel> States => _states;

        public GameStateViewModel SelectedState => _states[CurrentIndex];

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (value != _currentIndex)
                {
                    SetProperty(ref _currentIndex, value);
                    RaisePropertyChanged(nameof(SelectedState));
                }
            }
        }

        private bool _showHeatMap;
        public bool ShowHeatMap
        {
            get => _showHeatMap;
            set => SetProperty(ref _showHeatMap, value);
        }

        public int MaxStateIndex => _states.Count - 1;

        public BrainInfoViewModel Brain { get; }

        public GeneticModels.SimulationResult Model { get; }


        public SimulationResultViewModel(GeneticModels.SimulationResult result)
        {
            Model = result.ThrowIfNull(nameof(result));

            _states = ConvertStates(result);
            Brain = new BrainInfoViewModel(result.Brain);
        }

        private static ObservableCollection<GameStateViewModel> ConvertStates(
            GeneticModels.SimulationResult result)
        {
            var states = new ObservableCollection<GameStateViewModel>();
            foreach (var state in result.Results.SelectMany(r => r.States))
            {
                states.Add(new GameStateViewModel(state, result.Brain));
            }

            return states;
        }
    }
}
