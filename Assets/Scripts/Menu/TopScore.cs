using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopScore : MonoBehaviour
{
    public string PlayerName
    {
        get { return PlayerName; }
        set
        {
            playerName = value;
            UpdateText();
        }
    }

    public int PlayerScore
    {
        get { return PlayerScore; }
        set
        {
            playerScore = value;
            UpdateText();
        }
    }

    private TextMeshProUGUI scoreText;

    private string playerName = string.Empty;
    private int playerScore = 0;

    private bool initialized = false;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        initialized = true;
        UpdateText();
    }

    private void UpdateText()
    {
        Debug.Log(playerName);
        if (initialized)
            scoreText.text = $"{playerName} - {playerScore}";
    }
}
