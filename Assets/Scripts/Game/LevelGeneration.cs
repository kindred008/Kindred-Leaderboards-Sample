using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private GameObject ground;

    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private float platformSpacing = 2f;

    private float lastSpawnedHeight;

    private Camera mainCamera;

    private float screenWidth;
    private float screenHeight;

    private void Start()
    {
        lastSpawnedHeight = ground.transform.position.y;

        mainCamera = Camera.main;
        screenHeight = mainCamera.orthographicSize * 2f;
        screenWidth = screenHeight * mainCamera.aspect;
    }

    private void Update()
    {
        SpawnNewPlatforms();
    }

    private void SpawnNewPlatforms()
    {
        if (lastSpawnedHeight - mainCamera.transform.position.y < spawnHeight)
        {
            var spawnToHeight = lastSpawnedHeight + spawnHeight;
            for (float i = lastSpawnedHeight + platformSpacing; i < spawnToHeight; i += platformSpacing)
            {
                var spawnWidth = screenWidth / 2f;
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), i);

                var platformToSpawn = platforms[Random.Range(0, platforms.Length)];
                Instantiate(platformToSpawn, spawnPosition, Quaternion.identity);

                lastSpawnedHeight = i;
            }
        }
    }
}
