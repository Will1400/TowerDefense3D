using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Wave
{
    public List<GameObject> Enemies = new List<GameObject>();

    public int Difficulity;

    public float SpawnRate;
}