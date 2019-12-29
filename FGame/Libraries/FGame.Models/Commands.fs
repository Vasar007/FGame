module FGame.Models.Commands

type GameCommand =
    | MoveLeft
    | MoveRight
    | MoveUp
    | MoveDown
    | MoveUpLeft
    | MoveUpRight
    | MoveDownLeft
    | MoveDownRight
    | Wait
    | Restart
