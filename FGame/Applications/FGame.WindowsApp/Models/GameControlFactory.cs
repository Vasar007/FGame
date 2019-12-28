using System;
using System.Windows.Controls;
using Prism.Events;
using FGame.WindowsApp.Domain;
using FGame.WindowsApp.Models.GameStrategies;
using FGame.WindowsApp.ViewModels;
using FGame.WindowsApp.Views;

namespace FGame.WindowsApp.Models
{
    internal static class GameControlFactory
    {
        public static UserControl CreateControl(
            GameMode selectedMode,
            IEventAggregator eventAggregator)
        {
            return selectedMode switch
            {
                GameMode.User => MakeControlForUser(eventAggregator),

                GameMode.ArtificialIntelligence =>
                    MakeControlForArtificialIntelligence(eventAggregator),

                _ => throw new ArgumentOutOfRangeException(
                        nameof(selectedMode), selectedMode,
                        $"Unknown selected game mode: '{selectedMode.ToString()}'."
                    )
            };
        }

        private static UserControl MakeControlForUser(IEventAggregator eventAggregator)
        {
            var gameStrategy = new UserGameStrategy();
            var dataContext = new UserViewModel(gameStrategy, eventAggregator);
            return new UserView(dataContext);
        }

        private static UserControl MakeControlForArtificialIntelligence(
            IEventAggregator eventAggregator)
        {
            var gameStrategy = new ArtificialIntelligenceGameStrategy();
            var dataContext = new ArtificialIntelligenceViewModel(gameStrategy, eventAggregator);
            return new ArtificialIntelligenceView(dataContext);
        }
    }
}
