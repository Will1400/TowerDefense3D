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
            Instantiate(wave.Enemies[i], spawnPoints[0], Quaternion.identity, enemyHolder);
            yield return new WaitForSeconds(wave.SpawnRate);
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
        GameObject enemyPrefab = enemyPrefabs.First();
        float a = Random.Range(0.2f, 0.4f);
        float b = Random.Range(0.2f, 0.4f);
        float c = Random.Range(0.2f, 0.4f);
        float d = Random.Range(0.2f, 0.4f);

        float abcd = a + b + c + d;

        int enemyAmount = 6 + (int)(a / abcd) * 8 + difficulity;
        float spawnRate = 0.4f + (d / abcd) - difficulity * 0.002f;

        Wave wave = new Wave
        {
            Difficulity = difficulity,
            SpawnRate = spawnRate,
        };

        for (int i = 0; i < enemyAmount; i++)
        {

            a = Random.Range(0.2f, 0.4f);
            b = Random.Range(0.2f, 0.4f);
            c = Random.Range(0.2f, 0.4f);
            d = Random.Range(0.2f, 0.4f);
            abcd = a + b + c + d;
            float life = 50f + (b / abcd) * 10f + difficulity * 2f;
            float speed = 0.2f + (c / abcd) + difficulity * 0.10f;

            Enemy enemyComponent = enemyPrefab.GetComponent<Enemy>();
            enemyComponent.health = life;
            enemyComponent.speed = speed;
            wave.Enemies.Add(enemyComponent.gameObject);
        }

        return wave;
    }

    void GetSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.transform.position).ToList();
    }
}