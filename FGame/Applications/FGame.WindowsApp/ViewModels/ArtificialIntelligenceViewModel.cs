using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using FGame.DomainLogic;
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

        // Initialize this field inside Reset call in ctor.
        private States.GameState _state = default!;

        // Initialize this field inside RandomizeBrain call in ctor.
        private BrainInfoViewModel _brain = default!;
        public BrainInfoViewModel Brain
        {
            get => _brain;
            set
            {
                if (_brain != value)
                {
                    SetProperty(ref _brain, value.ThrowIfNull(nameof(value)));
                }
            }
        }

        public ICommand RandomizeCommand { get; }
        
        public ICommand BrainCommand { get; }

        public ICommand ResetCommand { get; }


        public ArtificialIntelligenceViewModel(
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            _random = new Random();
            _gameStrategy = new ArtificialIntelligenceGameStrategy(_random);

            RandomizeCommand = new DelegateCommand(RandomizeBrain);
            BrainCommand = new DelegateCommand(Move);
            ResetCommand = new DelegateCommand(Reset);

            RandomizeBrain();

            // Call reset method to update UI controls.
            Reset();
        }

        private void RandomizeBrain()
        {
            Brain = new BrainInfoViewModel(Genes.getRandomChromosome(_random));
        }

        private void Move()
        {
            _state = _gameStrategy.Move(_state, Brain);

            _eventAggregator
                  .GetEvent<UpdateGameStateMessage>()
                  .Publish(_state);
        }

        private void Reset()
        {
            _state = _gameStrategy.Reset(_state);

            _eventAggregator
                .GetEvent<UpdateGameStateMessage>()
                .Publish(_state);
        }
    }
}
