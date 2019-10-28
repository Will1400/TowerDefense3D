using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int[,] Map;
    public Transform[,] objMap;
    public int Seed = 10;

    [SerializeField]
    private Transform tilePrefab;
    [SerializeField]
    private GameObject startPointPrefab;
    [SerializeField]
    private GameObject endPointPrefab;

    [SerializeField]
    private Material pathMaterial;
    [SerializeField]
    private Vector2Int mapSize;
    [SerializeField, Range(0, 1)]
    private float outlinePercent;

    private Transform mapHolder;

    private AStar aStar;
    private Vector2Int startPoint;
    private Vector2Int endPoint;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        Setup();

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Map[x, y] = 0;
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + .5f + x, 0, -mapSize.y / 2 + .5f + y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity).transform;
                newTile.localScale = new Vector3(1 * (1 - outlinePercent), newTile.localScale.y, 1 * (1 - outlinePercent));
                newTile.localPosition += new Vector3(0, .5f - newTile.localScale.y / 2);
                newTile.SetParent(mapHolder.Find("Tiles"));
                objMap[x, y] = newTile;
            }
        }
        endPoint = GenerateEndPoint();
        startPoint = GenerateStartPoint();
        List<Coord> paths = aStar.GetPath(Map, new Coord(startPoint.x, startPoint.y), new Coord(endPoint.x, endPoint.y));

        foreach (var path in paths)
        {
            Debug.Log(path);
            //objMap[path.x, path.y].GetComponent<Renderer>().material = pathMaterial;
        }
    }

    void Setup()
    {
        aStar = new AStar();
        Random.InitState(Seed + (int)mapSize.x + (int)mapSize.y);

        if (mapSize.x < 4)
            mapSize.x = 4;
        if (mapSize.y < 4)
            mapSize.y = 4;

        Map = new int[mapSize.x, mapSize.y];
        objMap = new Transform[mapSize.x, mapSize.y];

        string holderName = "Generated Map";
        if (transform.Find(holderName))
            DestroyImmediate(transform.Find(holderName).gameObject);
        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        Transform tiles = new GameObject("Tiles").transform;
        tiles.parent = mapHolder;

        Transform objects = new GameObject("Objects").transform;
        objects.parent = mapHolder;

        Transform path = new GameObject("Path").transform;
        path.parent = mapHolder;
    }

    Vector2Int GenerateEndPoint()
    {
        int x = Random.Range(2, mapSize.x / 4);
        int y = Random.Range(2, mapSize.y / 4);
        Map[x, y] = 10;
        Instantiate(endPointPrefab, new Vector3(-mapSize.x / 2 + .5f + x, 1, -mapSize.y / 2 + .5f + y), Quaternion.identity, mapHolder.Find("Path"));

        return new Vector2Int(x, y);
    }

    Vector2Int GenerateStartPoint()
    {
        int x = Random.Range(1, mapSize.x - 1);
        int y = Random.Range(1, mapSize.y - 1);
        Map[x, y] = 9;
        Instantiate(startPointPrefab, new Vector3(-mapSize.x / 2 + .5f + x, 1, -mapSize.y / 2 + .5f + y), Quaternion.identity, mapHolder.Find("Path"));

        return new Vector2Int(x, y);
    }
}