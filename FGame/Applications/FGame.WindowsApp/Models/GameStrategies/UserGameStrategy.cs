using FGame.DomainLogic;
using FGame.WindowsApp.Domain;

namespace FGame.WindowsApp.Models.GameStrategies
{
    internal sealed class UserGameStrategy
    {
        public UserGameStrategy()
        {
        }

        public Simulator.GameState Move(Simulator.GameState currentState, string? direction)
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

        public Simulator.GameState Reset(Simulator.GameState? currentState)
        {
            return CreateDefaultState();
        }

        private static Simulator.GameState CreateDefaultState()
        {
            World.World world = WorldGeneration.makeDefaultWorld();
            const int turnsLeft = 30;
            return new Simulator.GameState(world, Simulator.SimulationState.Simulating, turnsLeft);
        }
    }
}
