module FGame.DomainLogic.WorldPos

open System

type WorldPos = {
    X: int32;
    Y: int32
}

let newPos (x: int32) (y: int32) =
    { X = x; Y = y }

let minPos = newPos 1 1
let maxPos = newPos 13 13

let createDefaultRandom =
    let random = Random()
    fun max -> (random.Next max) + 1

let isAdjacentTo (posA: WorldPos) (posB: WorldPos) =
    let xDiff = abs (posA.X - posB.X)
    let yDiff = abs (posA.Y - posB.Y)
    xDiff <= 1 && yDiff <= 1

let getRandomPos(maxX: int32, maxY: int32, getRandom: int32 -> int32) =
    let x = getRandom maxX 
    let y = getRandom maxY
    newPos x y

let getDistance(a: WorldPos, b: WorldPos) =
    let x1 = float(a.X)
    let x2 = float(b.X)
    let y1 = float(a.Y)
    let y2 = float(b.Y)

    // Calculate distance via C^2 = A^2 + B^2.
    System.Math.Sqrt((x1-x2)*(x1-x2) + (y1-y2)*(y1-y2))
