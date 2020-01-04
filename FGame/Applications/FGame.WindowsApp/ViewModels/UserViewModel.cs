using System.Windows.Input;
using Acolyte.Assertions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using FGame.WindowsApp.Domain.Messages;
using FGame.WindowsApp.Models.GameStrategies;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class UserViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly UserGameStrategy _gameStrategy;

        // Initialize this field inside Reset call in ctor.
        private SimulationResultViewModel _result = default!;

        public ICommand MoveCommand { get; }

        public ICommand ResetCommand { get; }


        public UserViewModel(
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            _gameStrategy = new UserGameStrategy();

            MoveCommand = new DelegateCommand<string?>(Move);
            ResetCommand = new DelegateCommand(Reset);

            // Call reset method to update UI controls.
            Reset();
        }

        private void Move(string? direction)
        {
            var simulationResult = _gameStrategy.Move(_result.SelectedState.State, direction);
            _result = new SimulationResultViewModel(simulationResult);

            _eventAggregator
                .GetEvent<UpdateBrainMessage>()
                .Publish(_result);
        }

        private void Reset()
        {
            var simulationResult = _gameStrategy.Reset();
            _result = new SimulationResultViewModel(simulationResult);

            _eventAggregator
                .GetEvent<UpdateBrainMessage>()
                .Publish(_result);
        }
    }
}
