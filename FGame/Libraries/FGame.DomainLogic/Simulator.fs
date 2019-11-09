﻿module FGame.DomainLogic.Simulator

open FGame.DomainLogic.Actors
open FGame.DomainLogic.Commands
open FGame.DomainLogic.World
open FGame.DomainLogic.WorldGeneration
open FGame.DomainLogic.WorldPos


type SimulationState =
    | Simulating
    | Won
    | Lost

type GameState =
    {
        World: World
        SimState: SimulationState
        TurnsLeft: int32
    }

    member this.Player = this.World.Squirrel

let stepSize = 1
let defaultTurnsLeft = 30

let canEnterActorCell (actor: ActorKind) (target: ActorKind) =
    match target with
        | Rabbit | Squirrel _ -> actor = Doggo // Dog can eat the squirrel or rabbit.
        | Doggo               -> false // Nobody bugs the dog.
        | Tree                -> actor = Squirrel true // Only allow if squirrel has an acorn.
        | Acorn               -> actor = Squirrel false // Only allow if squirrel w/o acorn.

let moveActor (state: GameState) (actor: Actor) (pos: WorldPos) = 
    let world = state.World

    let performMove =
        let actor = { actor with Pos = pos }
        match actor.Kind with
            | Squirrel _ -> { state with World = { world with Squirrel = actor } }
            | Tree       -> { state with World = { world with Tree     = actor } }
            | Acorn      -> { state with World = { world with Acorn    = actor } }
            | Rabbit     -> { state with World = { world with Rabbit   = actor } }
            | Doggo      -> { state with World = { world with Doggo    = actor } }

    let handleDogMove (state: GameState) (otherActor: Actor) =
        if otherActor.Kind = Rabbit then
            {
                state with
                    World = {
                        world with
                            Rabbit = { world.Rabbit with IsActive = false }
                            Doggo = { world.Doggo with Pos = pos }
                    }
            }
        else
            {
                state with
                    SimState = Lost
                    World = {
                        world with
                            Squirrel = { world.Squirrel with IsActive = false }
                            Doggo = { world.Doggo with Pos = pos }
                    }
            }

    let handleSquirrelMove (otherActor: Actor) (hasAcorn: bool) =
        if not hasAcorn && otherActor.Kind = Acorn && otherActor.IsActive then
            // Moving to the acorn for the first time should give the squirrel the acorn.
            {
                state with
                    World = {
                        world with
                            Squirrel = { Kind = Squirrel true; Pos = pos; IsActive = true } 
                            Acorn = { world.Acorn with IsActive = false }
                    }
            }
        else if hasAcorn && otherActor.Kind = Tree then
            // Moving to the tree with the acorn - this should win the game.
            {
                state with
                    SimState = Won
                    World = { 
                        world with Squirrel = { Kind = Squirrel true; Pos = pos; IsActive = true } 
                    }
            }
        else
            performMove

    let target = tryGetActor (pos.X, pos.Y) world

    match target with
        | None -> performMove
        | Some otherActor ->
            if otherActor <> actor && (canEnterActorCell actor.Kind otherActor.Kind) then 
                match actor.Kind with 
                    | Doggo             -> handleDogMove state otherActor
                    | Squirrel hasAcorn -> handleSquirrelMove otherActor hasAcorn
                    | _                 -> performMove
            else
                state

let getCandidates (current: WorldPos, world: World, includeCenter: bool) =
    let mutable (candidates: seq<WorldPos>) = Seq.empty
    for x in -1 .. 1 do
        for y in -1 .. 1 do
            if (includeCenter || x <> y || x <> 0) then
                // Make sure we're in the world boundaries.
                let candidatePos = {
                    X = current.X + x
                    Y = current.Y + y
                }
                if isValidPos candidatePos world then
                    candidates <- Seq.append candidates [ candidatePos ]
    candidates

let moveRandomly (state: GameState) (actor: Actor) (getRandomNumber: int32 -> int32) =
    let current = actor.Pos 
    let movedPos = getCandidates(current, state.World, false) 
                   |> Seq.sortBy (fun _ -> getRandomNumber 1000)
                   |> Seq.head

    moveActor state actor movedPos

let simulateDoggo (state: GameState) =
    let doggo = state.World.Doggo
    let rabbit = state.World.Rabbit
    let squirrel = state.World.Squirrel

    // Eat any adjacent actor.
    if rabbit.IsActive && isAdjacentTo doggo.Pos rabbit.Pos then
        moveActor state doggo rabbit.Pos
    else if squirrel.IsActive && isAdjacentTo doggo.Pos squirrel.Pos then
        moveActor state doggo squirrel.Pos
    else
        state

let decreaseTimer (state: GameState) =
    if state.SimState = Simulating then
        if state.TurnsLeft > 0 then
            { state with TurnsLeft = state.TurnsLeft - 1 }
        else
            { state with TurnsLeft = 0; SimState = Lost }
    else
        state

let simulateActors (state: GameState) (getRandomNumber: int32 -> int32) =
    moveRandomly state state.World.Rabbit getRandomNumber 
    |> simulateDoggo
    |> decreaseTimer

let handlePlayerCommand (state: GameState) (command: GameCommand) =
    let player = state.World.Squirrel
    let xDelta =
        match command with
            | MoveLeft  | MoveDownLeft  | MoveUpLeft  -> -stepSize
            | MoveRight | MoveDownRight | MoveUpRight ->  stepSize
            | _                                       ->  0
    let yDelta =
        match command with
            | MoveUpLeft   | MoveUp   | MoveUpRight   -> -stepSize
            | MoveDownLeft | MoveDown | MoveDownRight ->  stepSize
            | _                                       ->  0
    
    let movedPos = {
        X = player.Pos.X + xDelta
        Y = player.Pos.Y + yDelta
    }
    
    if isValidPos movedPos state.World then
        moveActor state player movedPos
    else
        state

let playTurn (state: GameState) (getRandomNumber: int32 -> int32) (command: GameCommand) =
    let world = state.World
    match command with 
        | Restart ->
            {
                World = makeWorld (world.MaxX, world.MaxY) getRandomNumber
                SimState = Simulating
                TurnsLeft = defaultTurnsLeft
            }
        | _       ->
            match state.SimState with
                | Simulating -> 
                    let newState = handlePlayerCommand state command 
                    simulateActors newState getRandomNumber
                | _          -> state