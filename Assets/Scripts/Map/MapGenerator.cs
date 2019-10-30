using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; set; }


    public int[,] Map; // 0 = tile, 1 = path, 2 = wall,  9 = start , 10 = end
    public Transform[,] objMap;
    public int Seed = 10;
    public List<Transform> Waypoints;

    public UnityEvent MapRendered;

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

    void Awake()
    {
        if (MapGenerator.Instance is null)
        {
            MapGenerator.Instance = this;
        }
        else if (MapGenerator.Instance != this)
        {
            Destroy(gameObject);
        }

        MapRendered = new UnityEvent();
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

                    case 1: // Path
                        newTile = Instantiate(pathPrefab, position, Quaternion.identity, mapHolder.Find("Path")).transform;
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        break;
                    case 9: // StartPoint
                        newTile = Instantiate(pathPrefab, position, Quaternion.identity, mapHolder.Find("Path")).transform;
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        Instantiate(startPointPrefab, new Vector3(-mapSize.x / 2 + .5f + x, 1, -mapSize.y / 2 + .5f + y), Quaternion.identity, mapHolder.Find("Path"));
                        break;
                    case 10: // EndPoint
                        newTile = Instantiate(pathPrefab, position, Quaternion.identity, mapHolder.Find("Path")).transform;
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        Instantiate(endPointPrefab, new Vector3(-mapSize.x / 2 + .5f + x, 1, -mapSize.y / 2 + .5f + y), Quaternion.identity, mapHolder.Find("Path"));
                        break;

                    default: // Normal tile
                        newTile = Instantiate(tilePrefab, position, Quaternion.identity, mapHolder.Find("Tiles")).transform;
                        newTile.localScale = new Vector3(1 * (1 - outlinePercent), newTile.localScale.y, 1 * (1 - outlinePercent));
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        newTile.name = $"Tile {x} {y}";
                        break;
                }
                objMap[x, y] = newTile;
            }
        }
        GenerateWaypoints();
        MapRendered.Invoke();
    }

    Vector2Int GenerateEndPoint()
    {
        int x = Random.Range(2, mapSize.x / 4);
        int y = Random.Range(2, mapSize.y / 4);
        Map[x, y] = 10;
        return new Vector2Int(x, y);
    }

    Vector2Int GenerateStartPoint()
    {
        int x = Random.Range(1, mapSize.x - 1);
        int y = Random.Range(1, mapSize.y - 1);
        Map[x, y] = 9;
        return new Vector2Int(x, y);
    }

    void GeneratePath(Vector2Int startPoint, Vector2Int endPoint)
    {
        Path = aStar.GetPath(Map, new Coord(startPoint.x, startPoint.y), new Coord(endPoint.x, endPoint.y));

        for (int i = 1; i < Path.Count - 1; i++)
        {
            Map[Path[i].x, Path[i].y] = 1;
        }
    }

    void GenerateWaypoints()
    {
        Waypoints = new List<Transform>();
        for (int i = 1; i < Path.Count - 1; i++)
        {
            Coord previous = Path[i - 1];
            Coord current = Path[i];
            Coord next = Path[i + 1];


            if (!AlignsWith(previous, next))
            {
                PlaceWaypoint(current);
            }

        }

        PlaceWaypoint(Path.First());

        bool AlignsWith(Coord first, Coord second)
        {
            if (first.x != second.x && first.y != second.y)
            {
                return false;
            }
            return true;
        }

        void PlaceWaypoint(Coord point)
        {
            var waypoint = new GameObject("Waypoint");
            Vector3 position = new Vector3(-mapSize.x / 2 + .5f + point.x, 1, -mapSize.y / 2 + .5f + point.y);
            waypoint.transform.position = position;
            waypoint.transform.SetParent(mapHolder.Find("Path"));
            Waypoints.Add(waypoint.transform);
        }
    }
}