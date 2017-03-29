namespace Tennis

type PlayerPoints = {player1: int; player2: int} with
    static member GetInstance(?player1Points: int, ?player2Points: int) =
        let orZero value = defaultArg value 0
        {player1 = player1Points |> orZero;
        player2 = player2Points |> orZero}

type ITennisGame =
    abstract Player1WonPoint : PlayerPoints -> PlayerPoints
    abstract Player2WonPoint : PlayerPoints -> PlayerPoints
    abstract GetScore : PlayerPoints -> string

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

type TennisGame1() =
    let ChainIfNone (ruleFunc: (unit -> Option<string>)) (maybeResult: Option<string>) =
            match maybeResult with
            | Some result -> Some(result)
            | None -> ruleFunc()
    interface ITennisGame with
        member this.Player1WonPoint(points) =
            {points with player1 = points.player1 + 1}
        member this.Player2WonPoint(points) =
            {points with player2 = points.player2 + 1}
        member this.GetScore(points) =
            let ChainStateIfNone = fun (ruleFunc: (PlayerPoints -> Option<string>)) -> ChainIfNone(fun unit -> ruleFunc(points))
            None
            |> ChainStateIfNone ScoreCalculationRules.GetScoreIfTieDuringRegularPlay
            |> ChainStateIfNone ScoreCalculationRules.GetScoreIfTieDuringExtendedPlay
            |> ChainStateIfNone ScoreCalculationRules.GetScoreIfWinOrAdvantage
            |> ChainStateIfNone ScoreCalculationRules.GetScoreForNormalPlayWhenNotATie
            |> Option.get
