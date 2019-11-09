module FGame.DomainLogic.Tests.Tests

open FsUnit
open Xunit
open FGame.DomainLogic.Simulator
open FGame.DomainLogic.World
open FGame.DomainLogic.WorldGeneration
open FGame.DomainLogic.WorldPos


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
