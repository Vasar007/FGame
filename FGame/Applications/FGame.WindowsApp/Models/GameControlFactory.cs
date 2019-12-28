using System;
using System.Windows.Controls;
using Prism.Events;
using FGame.WindowsApp.Domain;
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
            var dataContext = new UserViewModel(eventAggregator);
            return new UserView(dataContext);
        }

        private static UserControl MakeControlForArtificialIntelligence(
            IEventAggregator eventAggregator)
        {
            var dataContext = new ArtificialIntelligenceViewModel(eventAggregator);
            return new ArtificialIntelligenceView(dataContext);
        }
    }
}
