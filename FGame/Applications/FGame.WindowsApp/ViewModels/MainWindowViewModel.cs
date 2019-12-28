using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using FGame.DomainLogic;
using Prism.Commands;
using Prism.Mvvm;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private readonly ObservableCollection<ActorViewModel> _actors;

        public string GameStatusText => _state.SimState switch
        {
            Simulator.SimulationState.Won => "Won",
            Simulator.SimulationState.Lost => "Lost",
            _ => "Simulating"
        };

        public Brush GameStatusBrush => _state.SimState switch
        {
            Simulator.SimulationState.Won => Brushes.MediumSeaGreen,
            Simulator.SimulationState.Lost => Brushes.LightCoral,
            _ => Brushes.LightGray
        };

        public string TurnsLeftText => _state.TurnsLeft == 1
                ? "1 Turn Left"
                : $"{_state.TurnsLeft.ToString()} Turns Left";

        // Initialize this field inside Reset call in ctor (through State property).
        private Simulator.GameState _state = default!;
        public Simulator.GameState State
        {
            get => _state;
            set => UpdateState(value);
        }
        public IEnumerable<ActorViewModel> Actors => _actors;

        public ICommand ResetCommand { get; }

        public ICommand MoveCommand { get; }

        public MainWindowViewModel()
        {
            _actors = new ObservableCollection<ActorViewModel>();
            ResetCommand = new DelegateCommand(Reset);
            MoveCommand = new DelegateCommand<string?>(Move);

            Reset();
        }

        private void UpdateState(Simulator.GameState newState)
        {
            _state = newState ?? throw new ArgumentNullException(nameof(newState));

            _actors.Clear();
            foreach (var actor in _state.World.Actors.Where(a => a.IsActive))
            {
                _actors.Add(new ActorViewModel(actor));
            }

            RaisePropertyChanged(nameof(GameStatusBrush));
            RaisePropertyChanged(nameof(GameStatusText));
            RaisePropertyChanged(nameof(TurnsLeftText));
        }

        private void Reset()
        {
            World.World world = WorldGeneration.makeDefaultWorld();
            State = new Simulator.GameState(world, Simulator.SimulationState.Simulating, 30);
        }

        private void Move(string? direction)
        {
            // Parameter validation/cleansing.
            direction = direction is null
                ? string.Empty
                : direction.ToLowerInvariant();

            // Translate from the command parameter to the GameCommand in F#.
            Commands.GameCommand command = direction switch
            {
                "nw" => Commands.GameCommand.MoveUpLeft,
                "n"  => Commands.GameCommand.MoveUp,
                "ne" => Commands.GameCommand.MoveUpRight,
                "w"  => Commands.GameCommand.MoveLeft,
                "e"  => Commands.GameCommand.MoveRight,
                "sw" => Commands.GameCommand.MoveDownLeft,
                "s"  => Commands.GameCommand.MoveDown,
                "se" => Commands.GameCommand.MoveDownRight,
                _    => Commands.GameCommand.Wait
            };

            // Process the action and update our new state.
            State = Simulator.simulateTurn(_state, command);
        }
    }
}
