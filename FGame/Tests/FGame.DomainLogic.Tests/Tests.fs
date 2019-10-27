module FGame.DomainLogic.Tests.Tests

open System
open Xunit
open FGame.DomainLogic.WorldPos


[<Theory>]
[<InlineData(4, 2, 4, 1, true)>]
[<InlineData(4, 2, 4, 0, false)>]
let ``Point adjaency tests`` (x1, y1, x2, y2, expectedAdjancent) =
    // Arrange.
    let pos1 = newPos x1 y1
    let pos2 = newPos x2 y2

    // Act.
    let isAdjancent = isAdjacentTo pos1 pos2

    // Assert.
    Assert.Equal(expectedAdjancent, isAdjancent)
