module FGame.DomainLogic.WorldGeneration

open FGame.DomainLogic.Actors
open FGame.DomainLogic.World
open FGame.DomainLogic.WorldPos


let hasInvalidPlacedItems (items: seq<Actor>) (maxX: int32) (maxY: int32) =
    let mutable hasIssues = false

    for itemA in items do
        // Do not allow items to spawn in corners.
        let condition =
            (itemA.Pos.X = minPos.X || itemA.Pos.X = maxX) &&
            (itemA.Pos.Y = minPos.Y || itemA.Pos.Y = maxY)
        if condition then
            hasIssues <- true

        for itemB in items do
            if itemA <> itemB then
                // Do not allow two items to start next to each other.
                if isAdjacentTo itemA.Pos itemB.Pos then
                    hasIssues <- true

    hasIssues
    
let buildItemsArray (maxX: int32, maxY: int32, getRandom: int32 -> int32) =
    [
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Squirrel false; IsActive = true }
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Tree;           IsActive = true }
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Acorn;          IsActive = true }
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Rabbit;         IsActive = true }
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Doggo;          IsActive = true }
    ]

let generate (maxX: int32, maxY: int32, getRandom: int32 -> int32) =
    let mutable (items: list<Actor>) = buildItemsArray (maxX, maxY, getRandom)

    // It is possible to generate items in invalid starting configuration.
    // Make sure we do not do that.
    while hasInvalidPlacedItems items maxX maxY do
        items <- buildItemsArray (maxX, maxY, getRandom)

    items

let makeWorld (maxX: int32, maxY: int32) (getRandom: int32 -> int32) =
    let actors = generate (maxX, maxY, getRandom)
    {
        MaxX     = maxX
        MaxY     = maxY
        Squirrel = actors.[0]
        Tree     = actors.[1]
        Acorn    = actors.[2]
        Rabbit   = actors.[3]
        Doggo    = actors.[4]
    }

let makeDefaultWorld() =
    makeWorld (maxPos.X, maxPos.Y) createDefaultRandom

let makeTestWorld hasAcorn = 
    {
        MaxX     = maxPos.X
        MaxY     = maxPos.Y
        Squirrel = { Pos = { X = 1;  Y = 3  }; Kind = Squirrel hasAcorn; IsActive = true }
        Tree     = { Pos = { X = 8;  Y = 10 }; Kind = Tree;              IsActive = true }
        Doggo    = { Pos = { X = 2;  Y = 6  }; Kind = Doggo;             IsActive = true }
        Acorn    = { Pos = { X = 5;  Y = 7  }; Kind = Acorn;             IsActive = true }
        Rabbit   = { Pos = { X = 11; Y = 8  }; Kind = Rabbit;            IsActive = true }
    }
