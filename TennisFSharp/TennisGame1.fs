namespace Tennis

type ITennisGame =
    abstract WonPoint : string -> unit
    abstract GetScore : unit -> string

type TennisGameState = {player1Score: int; player2Score: int}

type TennisGame1() =
    let mutable _state:TennisGameState = {player1Score = 0; player2Score = 0}
    let ConvertScoreToString(score) =
        match score with
        | 0 -> Some("Love")
        | 1 -> Some("Fifteen")
        | 2 -> Some("Thirty")
        | 3 -> Some("Forty")
        | _ -> None
    interface ITennisGame with
        member this.WonPoint(playerName) =
            match playerName with
            | "player1" -> _state <- {_state with player1Score = _state.player1Score + 1 }
            | _ -> _state <- {_state with player2Score = _state.player2Score + 1 }
        member this.GetScore() =
            match _state with
            | {player1Score = player1Score; player2Score = player2Score} when player1Score = player2Score && player1Score < 3 ->
                ConvertScoreToString(player1Score).Value + "-" + "All"
            | {player1Score = player1Score; player2Score = player2Score} when player1Score = player2Score ->
                "Deuce"
            | {player1Score = player1Score; player2Score = player2Score} when player1Score >= 4 || player2Score >= 4 ->
                let diff = player1Score - player2Score
                match diff with
                | 1 -> "Advantage player1"
                | -1 -> "Advantage player2"
                | diff when diff >= 2 -> "Win for player1"
                | _ -> "Win for player2"
            | {player1Score = player1Score; player2Score = player2Score} ->
                ConvertScoreToString(player1Score).Value + "-" + ConvertScoreToString(player2Score).Value

