module FGame.DomainLogic.Commands

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
