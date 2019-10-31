using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; set; }

    [SerializeField]
    private GameObject enemyPrefab;

    private List<Vector3> spawnPoints;
    private Transform enemyHolder;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);

        enemyHolder = new GameObject("EnemyHolder").transform;
        MapGenerator.Instance.MapRendered.AddListener(OnMapRendered);
    }

    void OnMapRendered()
    {
        GetSpawnPoints();
        StopCoroutine("SpawnEnemies");
        foreach (Transform item in enemyHolder)
        {
            Destroy(item.gameObject);
        }
        StartCoroutine("SpawnEnemies");
    }

    IEnumerator SpawnEnemies()
    {
        if (spawnPoints.Count == 0)
           yield break;

        while (true)
        {
            Instantiate(enemyPrefab, spawnPoints[0], Quaternion.identity, enemyHolder);
            yield return new WaitForSeconds(2);
        }
    }

    void GetSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.transform.position).ToList();
    }
}