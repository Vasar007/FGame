﻿module FGame.Models.States

open FGame.Models.World


type SimulationState =
    | Simulating = 0
    | Won = 1
    | Lost = 2

type GameState =
    {
        World: World
        SimState: SimulationState
        TurnsLeft: int32
    }

    member this.Player = this.World.Squirrel
