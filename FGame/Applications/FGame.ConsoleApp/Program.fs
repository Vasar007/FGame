﻿module FGame.ConsoleApp.Program

open FGame.ConsoleApp.Display
open FGame.ConsoleApp.Input
open FGame.DomainLogic.Simulator
open FGame.DomainLogic.WorldGeneration
open FGame.Models.States
open FGame.Models.WorldPos


[<EntryPoint>]
let main _ =
    try
        try
            printfn "FGame started."

            let getRandomNumber = createDefaultRandom

            let world = makeTestWorld false

            let mutable state = {
                World = world
                SimState = SimulationState.Simulating
                TurnsLeft = 30
            }
            let mutable simulating = true

            while simulating do
                let userCommand = getUserInput(state) |> tryParseInput

                match userCommand with
                    | Some command -> 
                        match command with 
                            | Exit               -> simulating <- false
                            | Action gameCommand -> state <- playTurn state getRandomNumber gameCommand
                    | None ->
                        printfn "Invalid input."

            0 // return an integer exit code.
        with
            | ex ->
                printfn "Exception occured: %s" (ex.ToString())
                1 // return an integer exit code.
    finally
        printfn "FGame finished."
