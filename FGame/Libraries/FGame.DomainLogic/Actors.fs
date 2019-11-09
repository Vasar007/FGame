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
    IsActive : bool
}

let getChar (actor: Actor) =
    match actor.Kind with
        | Squirrel hasAcorn -> match hasAcorn with
                                   | true  -> 'S'
                                   | false -> 's'
        | Tree              -> 'T'
        | Acorn             -> 'a'
        | Rabbit            -> 'R'
        | Doggo             -> 'D'
