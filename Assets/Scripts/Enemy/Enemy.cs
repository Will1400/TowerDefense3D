using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        
        Coord end = MapGenerator.Instance.Path[0];
        agent.SetDestination(new Vector3(end.x, 1, end.y));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
