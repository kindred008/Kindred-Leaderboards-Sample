using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityEvent OnGameOver = new UnityEvent();

    public bool IsGameOver { get; private set; }

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
    }
}
