using FGame.DomainLogic;

namespace FGame.WindowsApp.Models.GameStrategies
{
    internal interface IGameStrategy
    {
        Simulator.GameState Move(Simulator.GameState currentState, string? direction);
        Simulator.GameState Reset(Simulator.GameState? currentState);
    }
}
