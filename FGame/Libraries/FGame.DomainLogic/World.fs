module FGame.DomainLogic.World

open FGame.DomainLogic.Actors
open FGame.DomainLogic.WorldPos


type World =
    {
        MaxX: int32
        MaxY: int32
        Squirrel: Actor
        Tree: Actor
        Acorn: Actor
        Rabbit: Actor
        Doggo: Actor
    }

    member this.Actors = [
        this.Squirrel
        this.Tree
        this.Acorn
        this.Rabbit
        this.Doggo
    ]

let tryGetActor (x, y) (world: World) =
    world.Actors 
    |> Seq.tryFind(fun actor -> actor.IsActive && actor.Pos.X = x && actor.Pos.Y = y)

let getCharacterAtCell (x, y) (world: World) =
    match tryGetActor(x,y) world with
        | Some actor -> getChar actor
        | None       -> '.'

let isValidPos (pos: WorldPos) (world: World) =
    pos.X >= minPos.X &&
    pos.Y >= minPos.Y &&
    pos.X <= world.MaxX &&
    pos.Y <= world.MaxY

let hasObstacle pos (world: World) : bool =
    world.Actors
    |> Seq.exists(fun actor -> pos = actor.Pos)
