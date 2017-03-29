namespace Tennis

type ITennisGame =
    abstract Player1WonPoint : unit -> ITennisGame
    abstract Player2WonPoint : unit -> ITennisGame
    abstract GetScore : unit -> string

type PlayerPoints = {player1: int; player2: int}

module ScoreCalculationRules =
    let ConvertScoreToString(score) =
        match score with
        | 0 -> Some("Love")
        | 1 -> Some("Fifteen")
        | 2 -> Some("Thirty")
        | 3 -> Some("Forty")
        | _ -> None
    let GetScoreIfTieDuringRegularPlay({player1 = player1Points; player2 = player2Points}) =
        if player1Points = player2Points && player1Points < 3 then
            Some(ConvertScoreToString(player1Points).Value + "-" + "All")
        else
            None
    let GetScoreIfTieDuringExtendedPlay({player1 = player1Points; player2 = player2Points}) =
        if player1Points = player2Points && player1Points >= 3 then
            Some("Deuce")
        else
            None
    let GetScoreIfWinOrAdvantage({player1 = player1Points; player2 = player2Points}) =
        if player1Points > 3 || player2Points > 3 then
            let diff = player1Points - player2Points
            match diff with
            | 1 -> Some("Advantage player1")
            | -1 -> Some("Advantage player2")
            | diff when diff >= 2 -> Some("Win for player1")
            | diff when diff <= -2 -> Some("Win for player2")
            | _ -> None
        else
            None
    let GetScoreForNormalPlayWhenNotATie({player1 = player1Points; player2 = player2Points}) =
        if player1Points <> player2Points && player1Points <= 3 then
            Some(ConvertScoreToString(player1Points).Value + "-" + ConvertScoreToString(player2Points).Value)
        else
            None


type TennisGame1(?player1Score: int, ?player2Score: int) =
    let orZero value = defaultArg value 0
    let _state:PlayerPoints = 
        {player1 = player1Score |> orZero;
         player2 = player2Score |> orZero}
    let ChainStateIfNone (ruleFunc: (PlayerPoints -> Option<string>)) (maybeResult: Option<string>) =
        match maybeResult with
        | Some result -> Some(result)
        | None -> ruleFunc(_state)
    interface ITennisGame with
        member this.Player1WonPoint() =
            TennisGame1(_state.player1 + 1, _state.player2) :> ITennisGame
        member this.Player2WonPoint() =
            TennisGame1(_state.player1, _state.player2 + 1) :> ITennisGame
        member this.GetScore() =
            None
            |> ChainStateIfNone ScoreCalculationRules.GetScoreIfTieDuringRegularPlay
            |> ChainStateIfNone ScoreCalculationRules.GetScoreIfTieDuringExtendedPlay
            |> ChainStateIfNone ScoreCalculationRules.GetScoreIfWinOrAdvantage
            |> ChainStateIfNone ScoreCalculationRules.GetScoreForNormalPlayWhenNotATie
            |> Option.get
