using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private int leaderboardId;
    [SerializeField] private string leaderboardPassword;

    private string kindredLeaderboardsBaseUrl = "https://localhost:8081/api";

    public IEnumerator GetScoresForLeaderboard()
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
        } else
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);

            var scoresWrapper = JsonUtility.FromJson<LeaderboardScoreWrapper>("{\"leaderboardScores\":" + response + "}");

            foreach (LeaderboardScoreDto score in scoresWrapper.leaderboardScores)
            {
                Debug.Log(score.playerDto.playerName + ": " + score.score);
            }
        }
    }

    public IEnumerator AddScoreToLeaderboard(LeaderboardScore leaderboardScore)
    {
        string url = kindredLeaderboardsBaseUrl + $"/LeaderboardScore/external/addscore/{leaderboardId}";
        string scoreParams = $"?PlayerDto.PlayerUniqueIdentifier={leaderboardScore.PlayerUniqueIdentifier}&" +
            $"PlayerDto.PlayerName={leaderboardScore.PlayerName}&Score={leaderboardScore.Score}";
        string requestBody = "{\"password\":\"" + leaderboardPassword + "\"}";

        UnityWebRequest request = UnityWebRequest.Post(url + scoreParams, requestBody, "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);

            var score = JsonUtility.FromJson<LeaderboardScoreDto>(response);

            Debug.Log(score.playerDto.playerName);
            Debug.Log(score.playerDto.playerUniqueIdentifier);
            Debug.Log(score.score);
            Debug.Log(score.leaderboardPosition);
        }
    }

    public IEnumerator GetPlayersScoreFromLeaderboard(string playerUniqueId)
    {
        string url = kindredLeaderboardsBaseUrl + $"/LeaderboardScore/external/getscore/{leaderboardId}";
        string urlParams = $"?playerUniqueId={playerUniqueId}";
        string requestBody = "{\"password\":\"" + leaderboardPassword + "\"}";

        UnityWebRequest request = UnityWebRequest.Post(url + urlParams, requestBody, "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string response = request.downloadHandler.text;
            Debug.Log(response);

            var score = JsonUtility.FromJson<LeaderboardScoreDto>(response);

            Debug.Log(score.playerDto.playerName);
            Debug.Log(score.playerDto.playerUniqueIdentifier);
            Debug.Log(score.score);
            Debug.Log(score.leaderboardPosition);
        }
    }
}

[System.Serializable]
public class LeaderboardScoreWrapper
{
    public LeaderboardScoreDto[] leaderboardScores;
}
 