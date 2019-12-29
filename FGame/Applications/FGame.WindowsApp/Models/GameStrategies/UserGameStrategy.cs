using FGame.DomainLogic;
using FGame.WindowsApp.Domain;

namespace FGame.WindowsApp.Models.GameStrategies
{
    internal sealed class UserGameStrategy
    {
        public UserGameStrategy()
        {
        }

        public States.GameState Move(States.GameState currentState, string? direction)
        {
            // Parameter validation/cleansing.
            currentState.ThrowIfNull(nameof(currentState));
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
            return Simulator.simulateTurn(currentState, command);
        }

        public States.GameState Reset(States.GameState? currentState)
        {
            return CreateDefaultState();
        }

        private static States.GameState CreateDefaultState()
        {
            World.World world = WorldGeneration.makeDefaultWorld();
            const int turnsLeft = 30;
            return new States.GameState(world, States.SimulationState.Simulating, turnsLeft);
        }
    }
}
