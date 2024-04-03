using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private int leaderboardId;
    [SerializeField] private string leaderboardPassword;

    private string kindredLeaderboardsBaseUrl = "https://localhost:8081/api";

    public IEnumerator GetScoresForLeaderboard(int id, string password)
    {
        UnityWebRequest request = new UnityWebRequest(kindredLeaderboardsBaseUrl + $"/LeaderboardScore/external/getscores/{id}", "POST");

        string requestBody = "{\"password\":\"" + password + "\"}";
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
}

[System.Serializable]
public class LeaderboardScoreWrapper
{
    public LeaderboardScoreDto[] leaderboardScores;
}
