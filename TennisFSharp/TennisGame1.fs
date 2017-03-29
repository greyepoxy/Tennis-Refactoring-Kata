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
    let GetScoreIfTieDuringRegularPlay({player1Score = player1Score; player2Score = player2Score}) =
        if player1Score = player2Score && player1Score < 3 then
            Some(ConvertScoreToString(player1Score).Value + "-" + "All")
        else
            None
    let GetScoreIfTieDuringExtendedPlay({player1Score = player1Score; player2Score = player2Score}) =
        if player1Score = player2Score && player1Score >= 3 then
            Some("Deuce")
        else
            None
    let GetScoreIfWinOrAdvantage({player1Score = player1Score; player2Score = player2Score}) =
        if player1Score > 3 || player2Score > 3 then
            let diff = player1Score - player2Score
            match diff with
            | 1 -> Some("Advantage player1")
            | -1 -> Some("Advantage player2")
            | diff when diff >= 2 -> Some("Win for player1")
            | diff when diff <= -2 -> Some("Win for player2")
            | _ -> None
        else
            None
    let GetScoreForNormalPlayWhenNotATie({player1Score = player1Score; player2Score = player2Score}) =
        if player1Score <> player2Score && player1Score <= 3 then
            Some(ConvertScoreToString(player1Score).Value + "-" + ConvertScoreToString(player2Score).Value)
        else
            None
    interface ITennisGame with
        member this.WonPoint(playerName) =
            match playerName with
            | "player1" -> _state <- {_state with player1Score = _state.player1Score + 1 }
            | _ -> _state <- {_state with player2Score = _state.player2Score + 1 }
        member this.GetScore() =
            let scoreRuleFuncs = [GetScoreIfTieDuringRegularPlay; GetScoreIfTieDuringExtendedPlay; GetScoreIfWinOrAdvantage; GetScoreForNormalPlayWhenNotATie]
            let matchingScoreRuleCalc = List.find (fun ruleFunc -> Option.isSome(ruleFunc(_state))) scoreRuleFuncs
            Option.get(matchingScoreRuleCalc(_state))
