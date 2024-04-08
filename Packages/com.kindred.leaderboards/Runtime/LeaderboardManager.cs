using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private int leaderboardId;
    [SerializeField] private string leaderboardPassword;

    private string kindredLeaderboardsBaseUrl = "https://localhost:8081/api";

    public void GetScoresForLeaderboard(Action<LeaderboardScoreDto[]> success, Action<string> failure)
    {
        StartCoroutine(GetScoresForLeaderboardCoroutine(success, failure));
    }

    private IEnumerator GetScoresForLeaderboardCoroutine(Action<LeaderboardScoreDto[]> success, Action<string> failure)
    {
        UnityWebRequest request = new UnityWebRequest(kindredLeaderboardsBaseUrl + $"/LeaderboardScore/external/getscores/{leaderboardId}", "POST");

        string requestBody = "{\"password\":\"" + leaderboardPassword + "\"}";
        byte[] requestBodyByes = System.Text.Encoding.UTF8.GetBytes(requestBody);

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(requestBodyByes);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);

            var response = request.downloadHandler.text;

            var failureMessage = string.IsNullOrEmpty(response) ? request.error : response;

            failure(failureMessage);
        } 
        else
        {
            string response = request.downloadHandler.text;

            var scoresWrapper = JsonUtility.FromJson<LeaderboardScoreWrapper>("{\"leaderboardScores\":" + response + "}");

            var leaderboardScores = scoresWrapper.leaderboardScores;
            success(leaderboardScores);
        }
    }

    public void AddScoreToLeaderboard(LeaderboardScore leaderboardScore, Action<LeaderboardScoreDto> success, Action<string> failure)
    {
        StartCoroutine(AddScoreToLeaderboardCoroutine(leaderboardScore, success, failure));
    }

    private IEnumerator AddScoreToLeaderboardCoroutine(LeaderboardScore leaderboardScore, Action<LeaderboardScoreDto> success, Action<string> failure)
    {
        string url = kindredLeaderboardsBaseUrl + $"/LeaderboardScore/external/addscore/{leaderboardId}";
        string scoreParams = $"?PlayerDto.PlayerUniqueIdentifier={leaderboardScore.PlayerUniqueIdentifier}&" +
            $"PlayerDto.PlayerName={leaderboardScore.PlayerName}&Score={leaderboardScore.Score}";
        string requestBody = "{\"password\":\"" + leaderboardPassword + "\"}";

        UnityWebRequest request = UnityWebRequest.Post(url + scoreParams, requestBody, "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            var response = request.downloadHandler.text;

            var failureMessage = string.IsNullOrEmpty(response) ? request.error : response;

            failure(failureMessage);
        }
        else
        {
            string response = request.downloadHandler.text;

            var score = JsonUtility.FromJson<LeaderboardScoreDto>(response);

            success(score);
        }
    }

    public void GetPlayersScoreFromLeaderboard(string playerUniqueId, Action<LeaderboardScoreDto> success, Action<string> failure)
    {
        StartCoroutine(GetPlayersScoreFromLeaderboardCoroutine(playerUniqueId, success, failure));
    }

    private IEnumerator GetPlayersScoreFromLeaderboardCoroutine(string playerUniqueId, Action<LeaderboardScoreDto> success, Action<string> failure)
    {
        string url = kindredLeaderboardsBaseUrl + $"/LeaderboardScore/external/getscore/{leaderboardId}";
        string urlParams = $"?playerUniqueId={playerUniqueId}";
        string requestBody = "{\"password\":\"" + leaderboardPassword + "\"}";

        UnityWebRequest request = UnityWebRequest.Post(url + urlParams, requestBody, "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            var response = request.downloadHandler.text;

            var failureMessage = string.IsNullOrEmpty(response) ? request.error : response;

            failure(failureMessage);
        }
        else
        {
            string response = request.downloadHandler.text;

            var score = JsonUtility.FromJson<LeaderboardScoreDto>(response);

            success(score);
        }
    }
}

[System.Serializable]
public class LeaderboardScoreWrapper
{
    public LeaderboardScoreDto[] leaderboardScores;
}
 