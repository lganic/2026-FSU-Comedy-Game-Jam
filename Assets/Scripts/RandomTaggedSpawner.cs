using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTaggedSpawner : MonoBehaviour
{
    [Header("Spawn Setup")]
    [SerializeField] string targetTag = "SpawnPoint";
    [SerializeField] GameObject prefabToSpawn;

    [Header("Timing (seconds)")]
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 4f;

    List<Transform> spawnPoints = new();

    void Start()
    {
        // Cache all transforms with the given tag
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (var go in taggedObjects)
            spawnPoints.Add(go.transform);

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning($"No objects found with tag '{targetTag}'");
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            SpawnAtRandomPoint();
        }
    }

    void SpawnAtRandomPoint()
    {
        Transform t = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(prefabToSpawn, t.position, t.rotation);
    }
}