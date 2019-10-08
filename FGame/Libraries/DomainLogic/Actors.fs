module FGame.DomainLogic.Actors

open FGame.DomainLogic.WorldPos


type ActorKind =
    | Squirrel of hasAcorn: bool
    | Tree
    | Acorn
    | Rabbit
    | Doggo

type Actor = {
    Pos: WorldPos
    Kind: ActorKind
}

let getChar (actor: Actor) =
    match actor.Kind with
        | Squirrel _ -> 'S'
        | Tree _     -> 'T'
        | Acorn _    -> 'a'
        | Rabbit _   -> 'R'
        | Doggo _    -> 'D'
