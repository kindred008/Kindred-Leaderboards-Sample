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
    [SerializeField] private float platformDestroyOffset = 1f;

    private List<GameObject> spawnedPlatforms;

    private float lastSpawnedHeight;
    private Vector3 lastSpawnedPosition;

    private Camera mainCamera;
    private float screenWidth;
    private float screenHeight;

    private void Awake()
    {
        spawnedPlatforms = new List<GameObject>();
    }

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
        RemovePlatforms();
    }

    private void SpawnNewPlatforms()
    {
        if (lastSpawnedHeight - mainCamera.transform.position.y < spawnHeight)
        {
            var spawnToHeight = lastSpawnedHeight + spawnHeight;
            float i = lastSpawnedHeight + platformSpacing;
            while (i < spawnToHeight)
            {
                var spawnWidth = screenWidth / 2f;

                var spawnY = Random.Range(i - 1, i + 1);
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), spawnY);

                while (Vector3.SqrMagnitude(lastSpawnedPosition - spawnPosition) > 50 &&
                    Vector3.SqrMagnitude(lastSpawnedPosition - new Vector3(spawnWidth, spawnPosition.y)) + Vector3.SqrMagnitude(new Vector3(-spawnWidth, spawnPosition.y) - spawnPosition) > 30 &&
                    Vector3.SqrMagnitude(lastSpawnedPosition - new Vector3(-spawnWidth, spawnPosition.y)) + Vector3.SqrMagnitude(new Vector3(spawnWidth, spawnPosition.y) - spawnPosition) > 30)
                {
                    spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), spawnY);
                }

                var platformToSpawn = platforms[Random.Range(0, platforms.Length)];
                var platform = Instantiate(platformToSpawn, spawnPosition, Quaternion.identity);

                spawnedPlatforms.Add(platform);

                i = lastSpawnedHeight = spawnY;
                lastSpawnedPosition = platform.gameObject.transform.position;

                i += platformSpacing;
            }
        }
    }


    private void RemovePlatforms()
    {
        var firstPlatform = spawnedPlatforms[0];

        if (firstPlatform.transform.position.y + platformDestroyOffset < mainCamera.transform.position.y - (screenHeight / 2f))
        {
            spawnedPlatforms.Remove(firstPlatform);
            Destroy(firstPlatform);
        }
    }
}
