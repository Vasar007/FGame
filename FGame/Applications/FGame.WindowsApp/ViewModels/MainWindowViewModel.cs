using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Prism.Mvvm;
using Prism.Events;
using FGame.DomainLogic;
using FGame.WindowsApp.Domain;
using FGame.WindowsApp.Domain.Messages;
using FGame.WindowsApp.Models;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly ObservableCollection<ActorViewModel> _actors;

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value.ThrowIfNull(nameof(value)));
        }

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

        private UserControl _gameContent = default!;
        public UserControl GameContent
        {
            get => _gameContent;
            set => SetProperty(ref _gameContent, value.ThrowIfNull(nameof(value)));
        }

        public IEnumerable<ActorViewModel> Actors => _actors;


        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            _actors = new ObservableCollection<ActorViewModel>();
            _title = DesktopOptions.Title;

            _eventAggregator
                .GetEvent<UpdateGameStateMessage>()
                .Subscribe(UpdateState);

            _gameContent = GameControlFactory.CreateControl(
                DesktopOptions.SelectedMode, eventAggregator
            );
        }

        private void UpdateState(Simulator.GameState newState)
        {
            _state = newState.ThrowIfNull(nameof(newState));

            _actors.Clear();
            foreach (var actor in _state.World.Actors.Where(a => a.IsActive))
            {
                _actors.Add(new ActorViewModel(actor));
            }

            RaisePropertyChanged(nameof(GameStatusBrush));
            RaisePropertyChanged(nameof(GameStatusText));
            RaisePropertyChanged(nameof(TurnsLeftText));
        }
    }
}
