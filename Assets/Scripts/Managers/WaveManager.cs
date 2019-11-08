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
        if (spawnPoints.Count == 0 && waves[Round -1] == null)
            yield break;

        for (int i = 0; i < waves[Round - 1].Enemies.Count; i++)
        {
            Wave wave = waves[Round - 1];
            if (wave.Enemies[i] != null)
            {
                wave.Enemies[i].transform.position = spawnPoints.First();
                wave.Enemies[i].SetActive(true);
                yield return new WaitForSeconds(wave.SpawnRate);
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        StartCoroutine(SpawnWave());
        while (enemyHolder.Find("Wave 1").childCount != 0)
        {
            yield return new WaitForSeconds(1f);
        }
        Round++;

        while (Round <= waves.Count)
        {
            if (enemyHolder.Find("Wave " + (Round - 1)) != null && enemyHolder.Find("Wave " + (Round - 1)).childCount == 0)
            {
                StartCoroutine(SpawnWave());
                yield return new WaitForSeconds(waves[Round - 1].SpawnRate * waves[Round - 1].Enemies.Count);
                Round++;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
        GameManager.Instance.GameWon.Invoke();
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
        float amount = Random.Range(0.2f, 0.4f);
        float b = Random.Range(0.2f, 0.4f);
        float c = Random.Range(0.2f, 0.4f);
        float d = Random.Range(0.2f, 0.4f);

        float abcd = amount + b + c + d;

        int enemyAmount = 6 + (int)(amount / abcd) * 13 + difficulity * 2;
        float spawnRate = 0.4f + (d / abcd) - difficulity * 0.01f;

        Wave wave = new Wave
        {
            Difficulity = difficulity,
            SpawnRate = spawnRate,
        };

        Transform waveHolder = new GameObject("Wave " + difficulity).transform;
        waveHolder.SetParent(enemyHolder);

        for (int i = 0; i < enemyAmount; i++)
        {

            b = Random.Range(0.2f, 0.8f);
            c = Random.Range(0.2f, 0.8f);
            abcd = amount + b + c + d;
            float life = 70f + (b * 3 / abcd) * 70f + difficulity * 20f;
            float speed = 0.5f + (c * 3 / abcd) * 1 + difficulity * 0.2f;

            Enemy enemyComponent = enemyPrefab.GetComponent<Enemy>();

            enemyComponent.health = life;
            enemyComponent.speed = speed;

            enemyPrefab.SetActive(false);
            GameObject enemyScaled = Instantiate(enemyPrefab, new Vector3(0, 1000, 0), Quaternion.identity, waveHolder);
            wave.Enemies.Add(enemyScaled);
        }

        return wave;
    }

    void GetSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.transform.position).ToList();
    }
}