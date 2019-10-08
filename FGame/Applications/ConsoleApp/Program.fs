module FGame.ConsoleApp.ConsoleApp

open System
open FGame.ConsoleApp.Display
open FGame.ConsoleApp.Input
open FGame.DomainLogic.Simulator
open FGame.DomainLogic.World

[<EntryPoint>]
let main _ =
    try
        printfn "FGame started."

        let getRandomNumber =
            let random = new Random()
            fun max -> (random.Next max) + 1

        let world = makeWorld worldSize getRandomNumber

        let mutable state = {
            World = world
        }
        let mutable simulating = true

        while simulating do
            let userCommand =
                getUserInput state.World
                |> tryParseInput

            match userCommand with
                | None -> printfn "Invalid input"
                | Some command ->
                    match command with
                        | Exit ->
                            simulating <- false
                        | Action gameCommand ->
                            state <- playTurn state getRandomNumber gameCommand

        0 // return an integer exit code.
    finally
        printfn "FGame finished."
