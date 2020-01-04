using System.Windows.Controls;
using Prism.Mvvm;
using Prism.Events;
using FGame.WindowsApp.Domain;
using FGame.WindowsApp.Domain.Messages;
using FGame.WindowsApp.Models;
using Acolyte.Assertions;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public string Title { get; }

        private SimulationResultViewModel? _brain;
        public SimulationResultViewModel? SelectedBrain
        {
            get => _brain;
            set => UpdateBrain(value);
        }

        private UserControl _gameContent = default!;
        public UserControl GameContent
        {
            get => _gameContent;
            set => SetProperty(ref _gameContent, value.ThrowIfNull(nameof(value)));
        }


        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            Title = DesktopOptions.Title;

            _eventAggregator
                .GetEvent<UpdateBrainMessage>()
                .Subscribe(UpdateBrain);

            _gameContent = GameControlFactory.CreateControl(
                DesktopOptions.SelectedMode, eventAggregator
            );
        }

        private void UpdateBrain(SimulationResultViewModel? newBrain)
        {
            SetProperty(ref _brain, newBrain, nameof(SelectedBrain));
        }
    }
}
