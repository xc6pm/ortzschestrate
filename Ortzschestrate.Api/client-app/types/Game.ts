export type Player = { userId: string; name: string }
export type PendingGame = {
  creator: Player
  creatorConnectionId: string
  gameType: string
  creatorColor: string
}

export type Game = {
  color: string
  opponent: string
}

export type GameEnding =
  | "Checkmate"
  | "Resigned"
  | "Timeout"
  | "Stalemate"
  | "DrawDeclared"
  | "InsufficientMaterial"
  | "FiftyMoveRule"
  | "Repetition"

export type GameResult = {
  result: GameEnding
  wonSide?: "w" | "b"
}
