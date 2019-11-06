using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.VFX;

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

                BuildManager.Instance.BuildEffect.transform.position = transform.position;
                BuildManager.Instance.BuildEffect.SetVector3("EffectPosition", transform.position);
                BuildManager.Instance.BuildEffect.SendEvent("OnPlay");

            }
            else
            {
                NodeUIController.Instance.SelectTile(this);
            }
        }
    }

    public void UpgradeTurret()
    {
        if (Turret is null)
        {
            State = TileState.Empty;
            return;
        }

        Turret.GetComponent<Turret>().Upgrade();
        NodeUIController.Instance.DeselectTile();
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