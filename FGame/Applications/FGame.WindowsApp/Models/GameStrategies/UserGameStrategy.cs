using Microsoft.FSharp.Collections;
using FGame.DomainLogic;
using FGame.Models;
using FGame.WindowsApp.Domain;

namespace FGame.WindowsApp.Models.GameStrategies
{
    internal sealed class UserGameStrategy
    {
        public UserGameStrategy()
        {
        }

        public GeneticModels.SimulationResult Move(States.GameState currentState, string? direction)
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
            var newState = Simulator.simulateTurn(currentState, command);

            return WrapStateToResult(newState);
        }

        public GeneticModels.SimulationResult Reset()
        {
            var defaultState = CreateDefaultState();

            return WrapStateToResult(defaultState);
        }

        private static States.GameState CreateDefaultState()
        {
            return new States.GameState(
                WorldGeneration.makeDefaultWorld(),
                States.SimulationState.Simulating,
                Simulator.defaultTurnsLeft
            );
        }

        private static GeneticModels.SimulationResult WrapStateToResult(States.GameState state)
        {
            // Use dirty trick with wrapping state into simulation result to use it in view.

            var states = new FSharpList<States.GameState>(
                state, FSharpList<States.GameState>.Empty
            );

            var indiviaualWorld = new GeneticModels.IndividualWorldResult(0.0, states);

            var results = new FSharpList<GeneticModels.IndividualWorldResult>(
                indiviaualWorld, FSharpList<GeneticModels.IndividualWorldResult>.Empty
            );

            var brain = new GeneticModels.ActorChromosome(FSharpList<double>.Empty, 0);

            return new GeneticModels.SimulationResult(0.0, results, brain);
        }
    }
}
