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


let getRandomPos (maxX: int32, maxY: int32, getRandom: int32 -> int32) =
    let x = getRandom maxX
    let y = getRandom maxY
    newPos x y

let buildItemsArray (maxX: int32, maxY: int32, getRandom: int32 -> int32) =
    [
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Squirrel false }
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Tree }
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Acorn }
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Rabbit }
        { Pos = getRandomPos (maxX, maxY, getRandom); Kind = Doggo }
    ]
 
let hasInvalidPlacedItems (items: list<Actor>) (maxX: int32) (maxY: int32) =
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

let generate (maxX: int32, maxY: int32, getRandom: int32 -> int32) =
    let mutable items = buildItemsArray (maxX, maxY, getRandom)

    // It is possible to generate items in invalid starting configuration.
    // Make sure we do not do that.
    while hasInvalidPlacedItems items maxX maxY do
        items <- buildItemsArray (maxX, maxY, getRandom)

    items

let makeWorld (maxX: int32, maxY: int32) (getRandom: int32 -> int32) =
    let actors = generate (maxX, maxY, getRandom)
    {
        MaxX = maxX
        MaxY = maxY
        Squirrel = actors.[0]
        Tree = actors.[1]
        Acorn = actors.[2]
        Rabbit = actors.[3]
        Doggo = actors.[4]
    }
