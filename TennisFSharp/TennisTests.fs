namespace TennisTests

open NUnit.Framework

[<TestFixture(0, 0, "Love-All")>]
[<TestFixture(1, 1, "Fifteen-All")>]
[<TestFixture(2, 2, "Thirty-All")>]
[<TestFixture(3, 3, "Deuce")>]
[<TestFixture(4, 4, "Deuce")>]
[<TestFixture(1, 0, "Fifteen-Love")>]
[<TestFixture(0, 1, "Love-Fifteen")>]
[<TestFixture(2, 0, "Thirty-Love")>]
[<TestFixture(0, 2, "Love-Thirty")>]
[<TestFixture(3, 0, "Forty-Love")>]
[<TestFixture(0, 3, "Love-Forty")>]
[<TestFixture(4, 0, "Win for player1")>]
[<TestFixture(0, 4, "Win for player2")>]
[<TestFixture(2, 1, "Thirty-Fifteen")>]
[<TestFixture(1, 2, "Fifteen-Thirty")>]
[<TestFixture(3, 1, "Forty-Fifteen")>]
[<TestFixture(1, 3, "Fifteen-Forty")>]
[<TestFixture(4, 1, "Win for player1")>]
[<TestFixture(1, 4, "Win for player2")>]
[<TestFixture(3, 2, "Forty-Thirty")>]
[<TestFixture(2, 3, "Thirty-Forty")>]
[<TestFixture(4, 2, "Win for player1")>]
[<TestFixture(2, 4, "Win for player2")>]
[<TestFixture(4, 3, "Advantage player1")>]
[<TestFixture(3, 4, "Advantage player2")>]
[<TestFixture(5, 4, "Advantage player1")>]
[<TestFixture(4, 5, "Advantage player2")>]
[<TestFixture(15, 14, "Advantage player1")>]
[<TestFixture(14, 15, "Advantage player2")>]
[<TestFixture(6, 4, "Win for player1")>]
[<TestFixture(4, 6, "Win for player2")>]
[<TestFixture(16, 14, "Win for player1")>]
[<TestFixture(14, 16, "Win for player2")>]
type TennisTests(player1Score: int, player2Score: int, expectedScore: string) =
    member this.player1Score = player1Score
    member this.player2Score = player2Score
    member this.expectedScore = expectedScore
    member this.CheckAllScores(game: Tennis.ITennisGame) =
        let highestScore = max this.player1Score this.player2Score
        for i in [0..highestScore] do
            if i < this.player1Score then
                game.WonPoint("player1")
            if (i < this.player2Score) then
                game.WonPoint("player2")
        Assert.AreEqual(this.expectedScore, game.GetScore())
    [<Test>]
    member this.CheckTennisGame1() =
        let game = Tennis.TennisGame1()
        this.CheckAllScores(game)

[<TestFixture>]
type ExampleGameTennisTest() =
    [<Test>]
    member this.CheckGame1() =
        let game = Tennis.TennisGame1()
        this.RealisticTennisGame(game)
    member this.RealisticTennisGame(game: Tennis.ITennisGame) =
        let points = [ "player1"; "player1"; "player2"; "player2"; "player1"; "player1" ]
        let expectedScores = [ "Fifteen-Love"; "Thirty-Love"; "Thirty-Fifteen"; "Thirty-All"; "Forty-Thirty"; "Win for player1" ]
        for i in [0..5] do
            game.WonPoint(points.[i])
            Assert.AreEqual(expectedScores.[i], game.GetScore())
