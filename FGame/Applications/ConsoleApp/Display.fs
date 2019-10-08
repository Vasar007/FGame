module FGame.ConsoleApp.Display

open System
open FGame.DomainLogic.Simulator
open FGame.DomainLogic.World


let printCell cellChar isLastCell =
    if isLastCell then
        printfn "%c" cellChar
    else
        printf "%c" cellChar

let displayWorld (world: World) =
    printfn ""

    for y in 1 .. world.MaxX do
        for x in 1 .. world.MaxY do
            // Determine which character should exist in this line.
            let cellChar = world |> getCharacterAtCell (x, y)
            printCell cellChar (x = world.MaxX)

let getUserInput (world: World) =
    displayWorld world
    printfn "%sPress R to regenerate or X to exit" Environment.NewLine

    let key = Console.ReadKey(true)
    Console.Clear()
    key
