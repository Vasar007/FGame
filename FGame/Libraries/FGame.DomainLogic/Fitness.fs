module FGame.DomainLogic.Fitness

open FGame.Models.Actors
open FGame.Models.States


let private getAcornBonus (state: GameState) =
    match state.World.Squirrel.Kind with 
        | Squirrel true -> 100.0
        | _             -> 0.0

let evaluateFitness (gameStates: list<GameState>, fitnessFunction) =
    fitnessFunction gameStates

let standardFitnessFunction (gameStates: list<GameState>) =
    let (lastState: GameState) = Seq.last gameStates

    let gameLength = float gameStates.Length

    let gotAcornBonus = getAcornBonus lastState

    let finalStateBonus =
        match lastState.SimState with
            | SimulationState.Won  -> 1000.0 - (gameLength * 10.0) // Reward quick wins.
            | _                    -> -50.0 + gameLength

    gotAcornBonus + finalStateBonus

let killRabbitFitnessFunction (gameStates: list<GameState>) =
    let (lastState: GameState) = Seq.last gameStates

    let gameLength = float gameStates.Length

    let gotAcornBonus = getAcornBonus lastState

    let isRabbitAlive = lastState.World.Rabbit.IsActive

    let finalStateBonus =
        match lastState.SimState with
        | SimulationState.Won  -> match isRabbitAlive with
                                      | false -> 1000.0 // Heavily reward dead rabbits.
                                      | true  -> 250.0 - (gameLength * 10.0) // Reward quick wins.
        | _                    -> match isRabbitAlive with
                                      | true -> -50.0 + gameLength
                                      | false -> gameLength

    gotAcornBonus + finalStateBonus
