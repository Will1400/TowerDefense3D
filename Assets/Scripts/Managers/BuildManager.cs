using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.VFX;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; set; }

    [SerializeField]
    public List<GameObject> TurretPrefabs;

    public GameObject SelectedTurret { get; set; }
    public VisualEffect BuildEffect;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SelectTurret(0);
    }

    public void SelectTurret(int index)
    {
        SelectedTurret = TurretPrefabs[index];
    }

}