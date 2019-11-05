using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public TileState State;

    public GameObject Turret;

    [Header("Materials")]
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material hoverMaterial;
    [SerializeField]
    private Material invalidMaterial;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void BuildTurret(GameObject _turret)
    {
        if (State == TileState.Empty)
        {
            Turret = _turret;
        }
    }

    private void OnMouseEnter()
    {
        meshRenderer.material = hoverMaterial;
    }
    private void OnMouseExit()
    {
        meshRenderer.material = defaultMaterial;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (State == TileState.Empty)
            {
                Turret = Instantiate(BuildManager.Instance.SelectedTurret, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity, GameObject.Find("Turrets").transform);
                Turret.transform.position += Turret.GetComponent<Turret>().offset;
                State = TileState.Filled;
                NodeUIController.Instance.DeselectTile();
            }
            else
            {
                NodeUIController.Instance.SelectTile(this);
            }
        }
    }

    public void SellTurret()
    {
        State = TileState.Empty;
        Destroy(Turret);
        Turret = null;
    }

    public Vector3 GetOffset()
    {
        return Turret.GetComponent<Turret>().offset;
    }
}