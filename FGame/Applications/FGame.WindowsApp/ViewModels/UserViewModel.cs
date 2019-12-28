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
    internal sealed class UserViewModel : BindableBase
    {
        private readonly IGameStrategy _gameStrategy;

        private readonly IEventAggregator _eventAggregator;

        // Initialize this field inside Reset call in ctor.
        private Simulator.GameState _state = default!;

        public ICommand MoveCommand { get; }

        public ICommand ResetCommand { get; }


        public UserViewModel(
            IGameStrategy gameStrategy,
            IEventAggregator eventAggregator)
        {
            _gameStrategy = gameStrategy.ThrowIfNull(nameof(gameStrategy));
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            MoveCommand = new DelegateCommand<string?>(Move);
            ResetCommand = new DelegateCommand(Reset);

            // Call reset method to update UI controls.
            Reset();
        }

        private void Move(string? direction)
        {
            _state = _gameStrategy.Move(_state, direction);

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
