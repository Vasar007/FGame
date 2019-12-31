using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using FGame.Models;
using FGame.WindowsApp.Domain;
using FGame.WindowsApp.Domain.Messages;
using FGame.WindowsApp.Models.GameStrategies;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class ArtificialIntelligenceViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly ArtificialIntelligenceGameStrategy _gameStrategy;

        private readonly Random _random;

        private SimulationResultViewModel? _brain;
        public SimulationResultViewModel? SelectedBrain
        {
            get => _brain;
            set
            {
                if (_brain != value)
                {
                    SetProperty(ref _brain, value);

                    // Disable warning because receiver should be able to process null values.
#pragma warning disable CS8604 // Possible null reference argument.
                    _eventAggregator
                        .GetEvent<UpdateBrainMessage>()
                        .Publish(_brain);
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }
        }

        public ICommand ResetCommand { get; }

        public ICommand RandomizeCommand { get; }
        
        public ICommand AdvanceCommand { get; }

        public ICommand Advance10Command { get; }

        public ObservableCollection<SimulationResultViewModel> Population { get; }


        public ArtificialIntelligenceViewModel(
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            _random = new Random();
            _gameStrategy = new ArtificialIntelligenceGameStrategy(_random);

            Population = new ObservableCollection<SimulationResultViewModel>();

            ResetCommand = new DelegateCommand(Reset);
            RandomizeCommand = new DelegateCommand(RandomizeWorlds);
            AdvanceCommand = new DelegateCommand(AdvanceToNextGeneration);
            Advance10Command = new DelegateCommand(AdvanceToNext10Generation);

            // Call this method to update UI controls.
            RandomizeBrains();
        }

        private void UpdatePopulation(IEnumerable<GeneticModels.SimulationResult> generation)
        {
            Population.Clear();
            foreach (var result in generation)
            {
                Population.Add(new SimulationResultViewModel(result));
            }

            SelectedBrain = Population.First();
        }

        private void RandomizeWorlds()
        {
            var result = _gameStrategy.RandomizeWorlds(Population);
            UpdatePopulation(result);
        }

        private void RandomizeBrains()
        {
            var result = _gameStrategy.RandomizeBrains();
            UpdatePopulation(result);
        }

        private void Reset()
        {
            RandomizeWorlds();
            RandomizeBrains();
        }

        private void AdvanceToNextGeneration()
        {
            var generation = _gameStrategy.AdvanceToNextGeneration(Population);
            UpdatePopulation(generation);
        }

        private void AdvanceToNext10Generation()
        {
            var generation = _gameStrategy.AdvanceToNext10Generation(Population);
            UpdatePopulation(generation);
        }
    }
}
