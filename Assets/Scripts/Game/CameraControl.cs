using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float riseSpeed = 1f;

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
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
    }
}
