using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; set; }


    public int[,] Map; // 0 = tile, 1 = path, 2 = wall,  9 = start , 10 = end
    public Transform[,] objMap;
    public int Seed = 10;

    [HideInInspector]
    public List<Coord> Path;

    [SerializeField]
    private Transform tilePrefab;
    [SerializeField]
    private Transform pathPrefab;
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
        if (MapGenerator.Instance is null)
        {
            MapGenerator.Instance = this;
        }
        else if (MapGenerator.Instance != this)
        {
            Destroy(gameObject);
        }

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
            }
        }

        endPoint = GenerateEndPoint();
        startPoint = GenerateStartPoint();
        GeneratePath(startPoint, endPoint);

        RenderMap();
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

    }




    void RenderMap()
    {
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

        Transform pathHolder = new GameObject("Path").transform;
        pathHolder.parent = mapHolder;


        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 position = new Vector3(-mapSize.x / 2 + .5f + x, 0, -mapSize.y / 2 + .5f + y);
                Transform newTile;
                switch (Map[x, y])
                {
                    case 0:
                        newTile = Instantiate(tilePrefab, position, Quaternion.identity, mapHolder.Find("Tiles")).transform;
                        newTile.localScale = new Vector3(1 * (1 - outlinePercent), newTile.localScale.y, 1 * (1 - outlinePercent));
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        objMap[x, y] = newTile;
                        newTile.name = $"Tile {x} {y}";
                        break;
                    case 1:
                        newTile = Instantiate(pathPrefab, position, Quaternion.identity, mapHolder.Find("Path")).transform;
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        break;
                }
            }
        }
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

    void GeneratePath(Vector2Int startPoint, Vector2Int endPoint)
    {
        Path = aStar.GetPath(Map, new Coord(startPoint.x, startPoint.y), new Coord(endPoint.x, endPoint.y));

        foreach (var path in Path)
        {
            Map[path.x, path.y] = 1;
            //var pathObj = objMap[path.x, path.y];
            //pathObj.GetComponent<Renderer>().material = pathMaterial;
            //pathObj.SetParent(mapHolder.Find("Path"));
            //pathObj.localScale = new Vector3(1, pathObj.localScale.y, 1);
            //pathObj.name = $"Path {path.x} {path.y}";
        }
    }
}