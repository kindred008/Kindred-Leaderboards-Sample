using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /*public bool speedMultiplierEnabled = false;

    [SerializeField] float riseSpeed = 0.5f;

    private float speedMultiplier = 2f;*/

    [SerializeField] Transform cameraTarget;
    [SerializeField] float smoothSpeed = 0.5f;

    private Vector3 currentPosition;

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
        /*var speed = speedMultiplierEnabled ? riseSpeed * speedMultiplier : riseSpeed;
        transform.Translate(Vector3.up * speed * Time.deltaTime);*/
    }

    private void LateUpdate()
    {
        if (cameraTarget.position.y + 0.5f > currentPosition.y)
        {
            currentPosition = new Vector3(transform.position.x, cameraTarget.position.y + 0.5f, transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position, currentPosition, smoothSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentPosition) < 0.05f)
        {
            transform.position = currentPosition;
        }
    }
}
