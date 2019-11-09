module FGame.DomainLogic.WorldPos

type WorldPos = {
    X: int32;
    Y: int32
}

let newPos (x: int32) (y: int32) =
    { X = x; Y = y }

let minPos = newPos 1 1
let maxPos = newPos 13 13

let isAdjacentTo (posA: WorldPos) (posB: WorldPos) =
    let xDiff = abs (posA.X - posB.X)
    let yDiff = abs (posA.Y - posB.Y)
    xDiff <= 1 && yDiff <= 1

let getRandomPos(maxX: int32, maxY: int32, getRandom: int32 -> int32) =
  let x = getRandom maxX 
  let y = getRandom maxY
  newPos x y
