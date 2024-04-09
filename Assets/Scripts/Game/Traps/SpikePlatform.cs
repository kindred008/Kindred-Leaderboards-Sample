using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlatform : MonoBehaviour
{
    [SerializeField] private SpikePlatformType type;
    [SerializeField] private GameObject spikeTrap;

    private List<Transform> spikeSpawns = new List<Transform>();

    private void Start()
    {
        foreach(Transform transform in transform)
        {
            spikeSpawns.Add(transform);
        }

        if (type == SpikePlatformType.PLATFORM)
        {
            SpawnSpikeTrapRandom();
        } else if (type == SpikePlatformType.TRAP)
        {
            SpawnSpikeTrapAll();
        }
    }

    private void SpawnSpikeTrapRandom()
    {
        var randIndex = Random.Range(0, spikeSpawns.Count);

        Instantiate(spikeTrap, spikeSpawns[randIndex]);
    }

    private void SpawnSpikeTrapAll()
    {
        foreach (Transform transform in spikeSpawns)
        {
            Instantiate(spikeTrap, transform);
        }
    }

    private enum SpikePlatformType
    {
        PLATFORM,
        TRAP
    }
}
