using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public GameObject fishPrefab; // Prefab of the fish
    public int maxFish = 4; // Maximum number of fish allowed
    public float spawnInterval = 20f; // Interval between fish spawns (in seconds)
    public float lastSpawnTime; // Time when the last fish was spawned
    public float spawnDistance = 10f; // Distance check for existing fish

    private void Start()
    {
     
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime > spawnInterval && CountFish() < maxFish && !IsFishWithinDistance(spawnDistance))
        {
            SpawnFish();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnFish()
    {
        // Spawn the fish at the position of the spawner
        Vector3 spawnPosition = transform.position; // Use the spawner's position
        Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
    }

    private int CountFish()
    {
        GameObject[] fish = GameObject.FindGameObjectsWithTag("fish");
        return fish.Length;
    }

    private bool IsFishWithinDistance(float distance)
    {
        GameObject[] fish = GameObject.FindGameObjectsWithTag("fish");

        foreach (GameObject f in fish)
        {
            if (Vector3.Distance(transform.position, f.transform.position) < distance)
            {
                return true; // Found a fish within the specified distance
            }
        }

        return false; // No fish found within the specified distance
    }
}
