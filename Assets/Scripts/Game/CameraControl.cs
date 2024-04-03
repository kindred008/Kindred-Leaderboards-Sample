using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public bool speedMultiplierEnabled = false;

    [SerializeField] float riseSpeed = 1f;

    private float speedMultiplier = 2f;

    private void OnEnable()
    {
        GameManager.OnGameOver.AddListener(GameOver);
    }

    private void OnDisable()
    {
        GameManager.OnGameOver.RemoveListener(GameOver);
    }

    private void GameOver()
    {
        enabled = false;
    }

    private void Update()
    {
        var speed = speedMultiplierEnabled ? riseSpeed * speedMultiplier : riseSpeed;
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
