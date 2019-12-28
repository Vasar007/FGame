using Prism.Events;
using Prism.Mvvm;
using FGame.WindowsApp.Domain;
using FGame.WindowsApp.Models.GameStrategies;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class ArtificialIntelligenceViewModel : BindableBase
    {
        private readonly IGameStrategy _gameStrategy;

        private readonly IEventAggregator _eventAggregator;


        public ArtificialIntelligenceViewModel(
            IGameStrategy gameStrategy,
            IEventAggregator eventAggregator)
        {
            _gameStrategy = gameStrategy.ThrowIfNull(nameof(gameStrategy));
            _eventAggregator = eventAggregator.ThrowIfNull(nameof(eventAggregator));

            //Reset();
        }
    }
}
