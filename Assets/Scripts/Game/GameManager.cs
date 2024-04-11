using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static UnityEvent OnGameOver = new UnityEvent();
    public static UnityEvent<int> OnScoreChanged = new UnityEvent<int>();

    public bool IsGameOver { get; private set; }

    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject GameOverUI;

    [SerializeField] private Transform trackScoreTransform;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverMessage;

    private LeaderboardScore highScore = null;

    public int Score
    {
        get { return score; }
        private set 
        { 
            if (value != score)
            {
                score = value;
                scoreText.text = "Score: " + score;
                OnScoreChanged.Invoke(score);
            }
        }
    }

    private int score = 0;
    private float trackScoreStartingY;

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void Start()
    {
        trackScoreStartingY = trackScoreTransform.position.y;

        LeaderboardManager.Instance.GetPlayersScoreFromLeaderboard(PlayerPrefs.GetString("PlayerID"), 
            playerScore =>
            {
                highScore = playerScore;
            }, 
            failureMessage =>
            {
                Debug.LogError(failureMessage);
            }
        );
    }

    private void Update()
    {
        Score = Mathf.FloorToInt(trackScoreTransform.position.y - trackScoreStartingY);
    }

    private void OnEnable()
    {
        OnGameOver.AddListener(GameOver);
    }

    private void OnDisable()
    {
        OnGameOver.RemoveListener(GameOver);
    }

    private void GameOver()
    {
        Debug.Log("Game over");
        IsGameOver = true;

        GameUI.SetActive(false);
        GameOverUI.SetActive(true);

        gameOverScoreText.text = score.ToString();

        LeaderboardScore leaderboardScore = new LeaderboardScore()
        {
            PlayerUniqueIdentifier = LeaderboardManager.Instance.GetPlayersUniqueID(),
            PlayerName = PlayerPrefs.GetString("PlayerName"),
            Score = score,
        };

        if ((highScore != null && leaderboardScore.Score > highScore.Score) || highScore == null)
        {
            LeaderboardManager.Instance.AddScoreToLeaderboard(leaderboardScore,
                success => {
                    gameOverMessage.gameObject.SetActive(true);
                    var rank = success.LeaderboardPosition;
                    gameOverMessage.text = "Well done! You placed " + rank + GetRankSuffix(rank);
                },
                failure => { Debug.LogError(failure); }
            );
        }  else if (highScore != null && leaderboardScore.Score < highScore.Score)
        {
            gameOverMessage.gameObject.SetActive(true);
            gameOverMessage.text = $"You failed to beat your high score of {highScore.Score}!";
        }
    }

    private string GetRankSuffix(int rank)
    {
        if (rank >= 11 && rank <= 13)
        {
            return "th";
        }

        switch (rank % 10)
        {
            case 1: return "st";
            case 2: return "nd";
            case 3: return "rd";
            default: return "th";
        }
    }
}
