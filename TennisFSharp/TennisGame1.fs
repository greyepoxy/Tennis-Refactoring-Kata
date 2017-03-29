namespace Tennis

type ITennisGame =
    abstract Player1WonPoint : unit -> ITennisGame
    abstract Player2WonPoint : unit -> ITennisGame
    abstract GetScore : unit -> string

type PlayerPoints = {player1: int; player2: int}

module ScoreCalculationRules =
    let ConverPointsToScore(points) =
        match points with
        | 0 -> Some("Love")
        | 1 -> Some("Fifteen")
        | 2 -> Some("Thirty")
        | 3 -> Some("Forty")
        | _ -> None
    let GetScoreIfTieDuringRegularPlay({player1 = player1Points; player2 = player2Points}) =
        if player1Points = player2Points && player1Points < 3 then
            Some(Option.get (ConverPointsToScore(player1Points)) + "-" + "All")
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
        if player1Points <> player2Points && player1Points <= 3 && player2Points <= 3 then
            Some(Option.get (ConverPointsToScore(player1Points)) + "-" + Option.get (ConverPointsToScore(player2Points)))
        else
            None


type TennisGame1(?player1Score: int, ?player2Score: int) =
    let orZero value = defaultArg value 0
    member this.Points:PlayerPoints = 
        {player1 = player1Score |> orZero;
         player2 = player2Score |> orZero}
    member private this.ChainStateIfNone (ruleFunc: (PlayerPoints -> Option<string>)) (maybeResult: Option<string>) =
        match maybeResult with
        | Some result -> Some(result)
        | None -> ruleFunc(this.Points)
    interface ITennisGame with
        member this.Player1WonPoint() =
            TennisGame1(this.Points.player1 + 1, this.Points.player2) :> ITennisGame
        member this.Player2WonPoint() =
            TennisGame1(this.Points.player1, this.Points.player2 + 1) :> ITennisGame
        member this.GetScore() =
            None
            |> this.ChainStateIfNone ScoreCalculationRules.GetScoreIfTieDuringRegularPlay
            |> this.ChainStateIfNone ScoreCalculationRules.GetScoreIfTieDuringExtendedPlay
            |> this.ChainStateIfNone ScoreCalculationRules.GetScoreIfWinOrAdvantage
            |> this.ChainStateIfNone ScoreCalculationRules.GetScoreForNormalPlayWhenNotATie
            |> Option.get
