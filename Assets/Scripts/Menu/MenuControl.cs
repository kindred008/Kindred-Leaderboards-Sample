using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private GameObject topScorePanel;
    [SerializeField] private GameObject topScorePrefab;
    [SerializeField] private Button playNavButton;
    [SerializeField] private Button scoreNavButton;
    [SerializeField] private TMP_InputField nameInputField;

    [Header("Players Score")]
    [SerializeField] private GameObject myTopScorePanel;
    [SerializeField] private TextMeshProUGUI myTopScoreText;

    public void UpdateName(string playerName)
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            nameInputField.text = playerName;
            PlayerPrefs.SetString("PlayerName", playerName);
            playNavButton.interactable = true;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenScoresPanel()
    {
        menuPanel.SetActive(false);
        scorePanel.SetActive(true);
    }

    public void ExitScoresPanel()
    {
        menuPanel.SetActive(true);
        scorePanel.SetActive(false);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            UpdateName(PlayerPrefs.GetString("PlayerName"));
        }

        LoadScores();
    }

    private void LoadScores()
    {
        LeaderboardManager.Instance.GetScoresForLeaderboard
            (
                scores =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i < scores.Length)
                        {
                            var topScoreObject = Instantiate(topScorePrefab, topScorePanel.transform);
                            var topScoreScript = topScoreObject.GetComponent<TopScore>();
                            topScoreScript.PlayerName = scores[i].PlayerName;
                            topScoreScript.PlayerScore = scores[i].Score;
                        }
                    }
                    scoreNavButton.interactable = true;
                },
                failure =>
                {
                    Debug.LogError(failure);
                }
            );
        LeaderboardManager.Instance.GetPlayersScoreFromLeaderboard(LeaderboardManager.Instance.GetPlayersUniqueID(),
                score =>
                {
                    myTopScoreText.text = score.Score.ToString();
                    myTopScorePanel.SetActive(true);
                },
                failure =>
                {
                    Debug.LogError(failure);
                }
            );
    }
}
