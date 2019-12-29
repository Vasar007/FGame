module FGame.DomainLogic.Tests.Tests

open FsUnit
open Xunit
open FGame.DomainLogic.Simulator
open FGame.DomainLogic.WorldGeneration
open FGame.Models.Actors
open FGame.Models.Commands
open FGame.Models.States
open FGame.Models.World
open FGame.Models.WorldPos


let getRandomNumber (max: int32) =
    42

let buildTestState =
    {
        World = (makeTestWorld false)
        SimState = SimulationState.Simulating
        TurnsLeft = defaultTurnsLeft
    }

[<Theory>]
[<InlineData(4, 2, 4, 1, true)>]
[<InlineData(4, 2, 4, 0, false)>]
let ``Point adjaency tests`` (x1, y1, x2, y2, expectedAdjancent) =
    // Arrange.
    let pos1 = newPos x1 y1
    let pos2 = newPos x2 y2

    // Act & Assert.
    isAdjacentTo pos1 pos2 |> should equal expectedAdjancent

[<Fact>]
let ``Rabbit should move randomly`` () =
    // Arrange.
    let state = buildTestState
    let originalPos = state.World.Rabbit.Pos

    // Act.
    let newWorld = simulateActors state getRandomNumber

    // Assert.
    newWorld.World.Rabbit.Pos |> should not' (equal originalPos)

[<Fact>]
let ``Dog should eat squirrel if adjacent`` () =
    // Arrange.
    let testState = buildTestState
    let squirrelPos = newPos (testState.World.Doggo.Pos.X + 1) testState.World.Doggo.Pos.Y
    (isValidPos squirrelPos testState.World) |> should be True

    let customSquirrel = {
        Pos = squirrelPos
        Kind = Squirrel false
        IsActive = true
    }
    let state = { testState with World = { testState.World with Squirrel = customSquirrel } }

    // Act.
    let newState = simulateActors state getRandomNumber

    // Assert.
    newState.World.Doggo.Pos |> should equal state.World.Squirrel.Pos
    newState.World.Squirrel.IsActive |> should equal false
    newState.SimState |> should equal SimulationState.Lost

[<Fact>]
let ``Simulating actors should decrease the turns left counter`` () =   
    // Arrange.
    let initialState = buildTestState

    // Act.
    let newState = simulateActors initialState getRandomNumber
  
    // Assert.
    newState.TurnsLeft |> should equal (initialState.TurnsLeft - 1)

[<Fact>]
let ``Running out of turns should lose the simulation`` () =
    // Arrange.
    let state = { buildTestState with TurnsLeft = 0 }
    
    // Act.
    let newState = simulateActors state getRandomNumber
  
    // Assert.
    newState.SimState |> should equal SimulationState.Lost

[<Fact>]
let ``Squirrel getting acorn should change how it displays`` () =
    // Arrange.
    let testState = buildTestState
    let squirrelPos = newPos (testState.World.Acorn.Pos.X + 1) testState.World.Acorn.Pos.Y
    (isValidPos squirrelPos testState.World) |> should be True

    let customSquirrel = {
        Pos = squirrelPos
        Kind = Squirrel false
        IsActive = true
    }
    let squirrelWithoutAcornChar = getChar customSquirrel
    let state = {
        testState with World = { testState.World with Squirrel = customSquirrel }
    }
  
    // Act.
    let newState = handlePlayerCommand state MoveLeft // MoveLeft to get acorn.
    let squirrelWithAcornChar = getChar newState.World.Squirrel

    // Assert.
    newState.World.Squirrel.Kind |> should equal (Squirrel true)
    squirrelWithAcornChar |> should not' (equal squirrelWithoutAcornChar)

[<Fact>]
let ``Squirrel getting acorn to tree should win game`` () =
    // Arrange.
    let testState = buildTestState
    let squirrelPos = newPos (testState.World.Tree.Pos.X + 1) testState.World.Tree.Pos.Y
    (isValidPos squirrelPos testState.World) |> should be True

    let customSquirrel = {
        Pos = squirrelPos
        Kind = Squirrel true
        IsActive = true
    }
    let state = {
        testState with World = { testState.World with Squirrel = customSquirrel }
    }
  
    // Act.
    let newState = handlePlayerCommand state MoveLeft // MoveLeft to climb to the tree.

    // Assert.
    newState.World.Squirrel.Pos |> should equal state.World.Tree.Pos
    newState.SimState |> should equal SimulationState.Won
