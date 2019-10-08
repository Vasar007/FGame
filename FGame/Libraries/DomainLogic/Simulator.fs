module FGame.DomainLogic.Simulator

open FGame.DomainLogic.Actors
open FGame.DomainLogic.World
open FGame.DomainLogic.WorldPos


type GameState =
    {
        World: World
    }

    member this.Player = this.World.Squirrel

let stepSize = 1

let worldSize = (13, 13)

let isInvalidPos (pos: WorldPos) (world: World) =
    pos.X >= minPos.X &&
    pos.Y >= minPos.Y &&
    pos.X <= world.MaxX &&
    pos.Y <= world.MaxY

let hasObstacle (pos: WorldPos) (world: World) =
    world.Actors
    |> Seq.exists (fun actor -> pos = actor.Pos)

let moveActor (world: World) (actor: Actor) (xDiff: int32) (yDiff: int32) =
    let pos = newPos (actor.Pos.X + xDiff) (actor.Pos.Y + yDiff)

    if (isInvalidPos pos world) && not (hasObstacle pos world) then
        let actor = { actor with Pos = pos }
        match actor.Kind with
            | Squirrel _ -> { world with Squirrel = actor }
            | Tree _     -> { world with Tree = actor }
            | Acorn _    -> { world with Acorn = actor }
            | Rabbit _   -> { world with Rabbit = actor }
            | Doggo _    -> { world with Doggo = actor }
    else
        world

let getCharacterAtCell (x: int32, y: int32) (world: World) =
    let actorAtCell =
        world.Actors
        |> Seq.tryFind (fun actor -> actor.Pos.X = x && actor.Pos.Y = y)

    match actorAtCell with
        | Some actor -> getChar actor
        | None -> '.'

type GameCommand =
    | MoveLeft
    | MoveRight
    | MoveUp
    | MoveDown
    | MoveUpLeft
    | MoveUpRight
    | MoveDownLeft
    | MoveDownRight
    | Wait
    | Restart

let playTurn (state: GameState) (getRandomNumber: int32 -> int32) (command: GameCommand) =
    let world = state.World
    let player = state.Player

    match command with
        | MoveLeft      -> { state with World = moveActor world player -stepSize  0        }
        | MoveRight     -> { state with World = moveActor world player  stepSize  0        }
        | MoveUp        -> { state with World = moveActor world player  0        -stepSize }
        | MoveDown      -> { state with World = moveActor world player  0         stepSize }
        | MoveUpLeft    -> { state with World = moveActor world player -stepSize -stepSize }
        | MoveUpRight   -> { state with World = moveActor world player  stepSize -stepSize }
        | MoveDownLeft  -> { state with World = moveActor world player -stepSize  stepSize }
        | MoveDownRight -> { state with World = moveActor world player  stepSize  stepSize }
        | Wait ->
            printfn "Time Passes..."
            state
        | Restart ->
            let newWorld = makeWorld worldSize getRandomNumber
            { World = newWorld }
