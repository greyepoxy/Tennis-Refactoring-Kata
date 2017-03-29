namespace TennisTests

open NUnit.Framework

[<TestFixture>]
type ShouldConvertPointsToScore() =
    [<Test>]
    member this.ZeroPointsIsLoveScore() =
        Assert.AreEqual(Some("Love"), Tennis.ScoreCalculationRules.ConverPointsToScore(0))
    [<Test>]
    member this.OnePointsIsFifteenScore() =
        Assert.AreEqual(Some("Fifteen"), Tennis.ScoreCalculationRules.ConverPointsToScore(1))
    [<Test>]
    member this.TwoPointsIsThirtyScore() =
        Assert.AreEqual(Some("Thirty"), Tennis.ScoreCalculationRules.ConverPointsToScore(2))
    [<Test>]
    member this.ThreePointsIsFortyScore() =
        Assert.AreEqual(Some("Forty"), Tennis.ScoreCalculationRules.ConverPointsToScore(3))
    [<Test>]
    member this.ThereIsNoScoreForGreaterThenThreePoints() =
        Assert.AreEqual(None, Tennis.ScoreCalculationRules.ConverPointsToScore(4))
        Assert.AreEqual(None, Tennis.ScoreCalculationRules.ConverPointsToScore(5))


[<TestFixture(0, 0, "Love-All")>]
[<TestFixture(1, 1, "Fifteen-All")>]
[<TestFixture(2, 2, "Thirty-All")>]
type ShouldDetectRegularPlayTieScore(player1Points, player2Points, expectedResult: string) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfTieDuringRegularPlay({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(Some(expectedResult), result)

[<TestFixture(0, 1)>]
[<TestFixture(1, 2)>]
type ShouldNotDetectRegularPlayTieScoreWhenNotATie(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfTieDuringRegularPlay({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(None, result)

[<TestFixture(3, 3)>]
[<TestFixture(4, 4)>]
type ShouldNotDetectRegularPlayTieScoreWhenNotInRegularPlay(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfTieDuringRegularPlay({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(None, result)

[<TestFixture(3, 3)>]
[<TestFixture(4, 4)>]
[<TestFixture(5, 5)>]
[<TestFixture(10, 10)>]
type ShouldReportDueceWhenExtendedPlayTieScore(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfTieDuringExtendedPlay({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(Some("Deuce"), result)

[<TestFixture(2, 2)>]
[<TestFixture(1, 1)>]
[<TestFixture(0, 0)>]
type ShouldNotReportDueceWhenRegularPlayTieScore(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfTieDuringExtendedPlay({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(None, result)

[<TestFixture(0, 1)>]
[<TestFixture(3, 4)>]
[<TestFixture(3, 5)>]
type ShouldNotReportDueceWhenNotATieScore(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfTieDuringExtendedPlay({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(None, result)

[<TestFixture(4, 3)>]
[<TestFixture(5, 4)>]
[<TestFixture(6, 5)>]
type ShouldReportPlayer1AdvantageWhenExtendedPlayAdvantageScore(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfWinOrAdvantage({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(Some("Advantage player1"), result)

[<TestFixture(3, 4)>]
[<TestFixture(4, 5)>]
[<TestFixture(5, 6)>]
type ShouldReportPlayer2AdvantageWhenExtendedPlayAdvantageScore(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfWinOrAdvantage({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(Some("Advantage player2"), result)

[<TestFixture(2, 3)>]
[<TestFixture(3, 2)>]
[<TestFixture(1, 3)>]
[<TestFixture(3, 1)>]
[<TestFixture(5, 5)>]
type ShouldNotReportAdvantageOrWinWhenRegularPlayOrTieDuringExtendedPlay(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfWinOrAdvantage({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(None, result)

[<TestFixture(5, 3)>]
[<TestFixture(6, 4)>]
[<TestFixture(7, 5)>]
[<TestFixture(9, 5)>]
type ShouldReportPlayer1Win(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfWinOrAdvantage({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(Some("Win for player1"), result)

[<TestFixture(3, 5)>]
[<TestFixture(4, 6)>]
[<TestFixture(5, 7)>]
[<TestFixture(5, 7)>]
[<TestFixture(5, 9)>]
type ShouldReportPlayer2Win(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreIfWinOrAdvantage({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(Some("Win for player2"), result)

[<TestFixture(1, 0, "Fifteen-Love")>]
[<TestFixture(0, 1, "Love-Fifteen")>]
[<TestFixture(2, 0, "Thirty-Love")>]
[<TestFixture(0, 2, "Love-Thirty")>]
[<TestFixture(3, 0, "Forty-Love")>]
[<TestFixture(0, 3, "Love-Forty")>]
[<TestFixture(2, 1, "Thirty-Fifteen")>]
[<TestFixture(1, 2, "Fifteen-Thirty")>]
[<TestFixture(3, 1, "Forty-Fifteen")>]
[<TestFixture(1, 3, "Fifteen-Forty")>]
[<TestFixture(3, 2, "Forty-Thirty")>]
[<TestFixture(2, 3, "Thirty-Forty")>]
type ShouldReportNormalPlayNonTieingScore(player1Points, player2Points, expectedResult: string) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreForNormalPlayWhenNotATie({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(Some(expectedResult), result)

[<TestFixture(0, 0)>]
[<TestFixture(3, 3)>]
[<TestFixture(3, 4)>]
[<TestFixture(4, 3)>]
[<TestFixture(4, 4)>]
type ShouldNotReportNormalPlayNonTieingScoreWhenNotNormalPlayOrPlayersAreTied(player1Points, player2Points) =
    [<Test>]
    member this.Validate() =
        let result = Tennis.ScoreCalculationRules.GetScoreForNormalPlayWhenNotATie({player1 = player1Points; player2 = player2Points})
        Assert.AreEqual(None, result)
