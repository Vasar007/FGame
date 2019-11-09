﻿module FGame.ConsoleApp.Display

open System
open FGame.DomainLogic.Simulator
open FGame.DomainLogic.World


let printCell cellChar isLastCell =
    if isLastCell then
        printfn "%c" cellChar
    else
        printf "%c" cellChar

let displayWorld (world: World) turnsRemaining =
    printfn ""
    printfn "%i turn(s) remaining" turnsRemaining
    printfn ""

    for y in 1..world.MaxX do
        printf "  " // Indent slightly so it is a bit more readable.

        for x in 1..world.MaxY do   
            // Determine which character should exist in this line.
            let char = world |> getCharacterAtCell(x, y)

            // Let's set the overall color.
            match char with
                | '.'       -> Console.ForegroundColor <- ConsoleColor.DarkGreen
                | 't'       -> Console.ForegroundColor <- ConsoleColor.Green
                | 'a'       -> Console.ForegroundColor <- ConsoleColor.DarkYellow
                | 'S' | 's' -> Console.ForegroundColor <- ConsoleColor.Yellow
                | 'D'       -> Console.ForegroundColor <- ConsoleColor.Red
                | 'R'       -> Console.ForegroundColor <- ConsoleColor.Magenta
                | _         -> Console.ForegroundColor <- ConsoleColor.Gray

            printCell char (x = world.MaxX)

    // Ensure we go back to standard format.
    Console.ForegroundColor <- ConsoleColor.White

let getUserInput (state: GameState) =
    displayWorld state.World state.TurnsLeft
    printfn ""
  
    match state.SimState with
        | Simulating -> printfn "Press Arrow Keys to move, R to regenerate, or X to exit"
        | Won        -> printfn "Squirrel Returned Home with the Acorn! Press R to reload or X to exit."
        | Lost       -> printfn "Simulation ended: Squirrel died. Press R to reload or X to exit."

    let key = Console.ReadKey(true)
    Console.Clear()
    key
