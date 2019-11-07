using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; set; }

    public int Round = 1;

    [SerializeField]
    private List<GameObject> enemyPrefabs;
    [SerializeField]
    private bool generateWaves = true;
    [SerializeField]
    private List<Wave> waves;

    private List<Vector3> spawnPoints;
    private Transform enemyHolder;


    void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);

        enemyHolder = new GameObject("EnemyHolder").transform;
    }

    private void Start()
    {
        MapGenerator.Instance.MapRendered.AddListener(OnMapRendered);
        GameManager.Instance.GameLost.AddListener(StopAllCoroutines);

    }

    void OnMapRendered()
    {
        GetSpawnPoints();
        StopCoroutine("SpawnWaves");
        foreach (Transform item in enemyHolder)
        {
            Destroy(item.gameObject);
        }

        if (generateWaves)
            GenerateWaves();

        StartCoroutine("SpawnWaves");
    }

    IEnumerator SpawnWave()
    {
        if (spawnPoints.Count == 0)
            yield break;

        for (int i = 0; i < waves[Round - 1].Enemies.Count; i++)
        {
            Wave wave = waves[Round - 1];
            for (int d = wave.Difficulity + i + 3; d > 0; d--)
            {
                Instantiate(wave.Enemies[i], spawnPoints[0], Quaternion.identity, enemyHolder);
                yield return new WaitForSeconds(wave.SpawnRate);
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        while (Round < waves.Count)
        {
            if (enemyHolder.childCount == 0)
            {
                Round++;
                yield return new WaitForSeconds(.1f);
                StartCoroutine(SpawnWave());
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    void GenerateWaves()
    {
        if (waves is null)
            waves = new List<Wave>();
        else
            waves.Clear();

        for (int i = 0; i < 20; i++)
        {
            waves.Add(GenerateWave(i + 1));
        }
    }

    Wave GenerateWave(int difficulity)
    {
        Wave wave = new Wave
        {
            Difficulity = difficulity,
            SpawnRate = .3f,
            Enemies = enemyPrefabs.Take(difficulity).ToList()
        };

        return wave;
    }

    void GetSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.transform.position).ToList();
    }
}