module FGame.ConsoleApp.Input

open System
open FGame.DomainLogic.Simulator

type Command =
    | Action of GameCommand
    | Exit

let tryParseInput (info: ConsoleKeyInfo) =
    match info.Key with
        | ConsoleKey.LeftArrow  -> Some (Action MoveLeft)
        | ConsoleKey.RightArrow -> Some (Action MoveRight)
        | ConsoleKey.UpArrow    -> Some (Action MoveUp)
        | ConsoleKey.DownArrow  -> Some (Action MoveDown)
        | ConsoleKey.NumPad7 | ConsoleKey.Home     -> Some (Action MoveUpLeft)
        | ConsoleKey.NumPad9 | ConsoleKey.PageUp   -> Some (Action MoveUpRight)
        | ConsoleKey.NumPad1 | ConsoleKey.End      -> Some (Action MoveDownLeft)
        | ConsoleKey.NumPad3 | ConsoleKey.PageDown -> Some (Action MoveDownRight)
        | ConsoleKey.NumPad5 | ConsoleKey.Spacebar -> Some (Action Wait)
        | ConsoleKey.X -> Some Exit
        | ConsoleKey.R -> Some (Action Restart)
        | _ -> None
