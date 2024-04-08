using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScore
{
    public string PlayerUniqueIdentifier { get; set; }
    public string PlayerName { get; set; }
    public int Score { get; set; }
    public int LeaderboardPosition { get; set; }

    public static LeaderboardScore FromDto(LeaderboardScoreDto scoreDto)
    {
        var leaderboardScore = new LeaderboardScore() 
        {
            PlayerUniqueIdentifier = scoreDto.playerDto.playerUniqueIdentifier,
            PlayerName = scoreDto.playerDto.playerName,
            Score = scoreDto.score,
            LeaderboardPosition = scoreDto.leaderboardPosition,
        };

        return leaderboardScore;
    }
}
