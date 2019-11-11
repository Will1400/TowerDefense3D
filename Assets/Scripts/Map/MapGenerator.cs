using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    [SerializeField, Range(0, 10)]
    private int pathDeviation = 1;
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
    private Vector2Int mapSize = new Vector2Int(20, 20);
    [SerializeField, Range(0, 1)]
    private float outlinePercent;

    private Transform mapHolder;

    private AStar aStar;
    private Vector2Int startPoint;
    private Vector2Int endPoint;

    void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        MapRendered = new UnityEvent();
    }
    
    public void Start()
    {
        GenerateMap();
        StartCoroutine(RenderMap());
    }

    public void GenerateMap()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Setup();

        endPoint = GenerateEndPoint();
        startPoint = GenerateStartPoint();
        GeneratePath(startPoint, endPoint);
        stopwatch.Stop();
        UnityEngine.Debug.Log("Generating map took: " + stopwatch.ElapsedMilliseconds);
    }

    void Setup()
    {
        aStar = new AStar();
        Path = new List<Coord>();
        Random.InitState(Seed + (int)mapSize.x + (int)mapSize.y);

        if (mapSize.x < 4)
            mapSize.x = 4;
        if (mapSize.y < 4)
            mapSize.y = 4;

        Map = new int[mapSize.x, mapSize.y];
    }

    public IEnumerator RenderMap()
    {
        Stopwatch renderTime = new Stopwatch();
        renderTime.Start();
        UnityEngine.Debug.Log("Started rendering map");

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

        if (GameObject.Find("Turrets"))
            DestroyImmediate(GameObject.Find("Turrets"));
        _ = new GameObject("Turrets").transform;

        Transform start = null;
        Transform end = null;

        yield return null; // Wait for next frame
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
                        newTile.name = $"Path {x} {y}";
                        break;
                    case 9: // StartPoint
                        newTile = Instantiate(pathPrefab, position, Quaternion.identity, mapHolder.Find("Path")).transform;
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        start = Instantiate(startPointPrefab, new Vector3(-mapSize.x / 2 + .5f + x, 1, -mapSize.y / 2 + .5f + y), Quaternion.identity, mapHolder.Find("Path")).transform;
                        break;
                    case 10: // EndPoint
                        newTile = Instantiate(pathPrefab, position, Quaternion.identity, mapHolder.Find("Path")).transform;
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        end = Instantiate(endPointPrefab, new Vector3(-mapSize.x / 2 + .5f + x, 1, -mapSize.y / 2 + .5f + y), Quaternion.identity, mapHolder.Find("Path")).transform;
                        break;

                    default: // Normal tile
                        newTile = Instantiate(tilePrefab, position, Quaternion.identity, mapHolder.Find("Tiles")).transform;
                        newTile.localScale = new Vector3(1 * (1 - outlinePercent), newTile.localScale.y, 1 * (1 - outlinePercent));
                        newTile.localPosition += new Vector3(0, .5f - newTile.localPosition.y / 2);
                        newTile.name = $"Tile {x} {y}";
                        break;
                }
                objMap[x, y] = newTile;

                if (renderTime.ElapsedMilliseconds % 200 == 0) // To make sure unity dosnt freeze
                    yield return null; // Wait for next frame
            }
        }

        GenerateWaypoints();
        start.LookAt(Waypoints.First());
        end.LookAt(Waypoints.Last());

        UnityEngine.Debug.Log("Rendering map took: " + renderTime.ElapsedMilliseconds);
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
        List<Coord> points = new List<Coord>();

        points.Add(new Coord(startPoint.x, startPoint.y));
        for (int i = 0; i < pathDeviation; i++)
        {
            points.Add(new Coord(Random.Range(0, mapSize.x), Random.Range(0, mapSize.y)));
        }
        points.Add(new Coord(endPoint.x, endPoint.y));

        for (int i = 0; i < points.Count - 1; i++)
        {
            Coord start = points[i];
            Coord end = points[i + 1];

            var newPaths = aStar.GetPath(Map, start, end);
            newPaths.Reverse();

            foreach (Coord item in newPaths)
            {
                if (!Path.Contains(item))
                    Path.Add(item);
            }
        }

        for (int i = 1; i < Path.Count - 1; i++)
        {
            if (Map[Path[i].x, Path[i].y] != 9 && Map[Path[i].x, Path[i].y] != 10)
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

        PlaceWaypoint(Path.Last());

        bool AlignsWith(Coord first, Coord second)
        {
            if (first.x != second.x && first.y != second.y)
            {
                return false;
            }
            return true;
        }
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