using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.VFX;
using System.Linq;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    [SerializeField]
    private List<GameObject> Effects;

    private void Awake()
    {
        if (instance is null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        string holderName = "Effects";
        if (transform.Find(holderName))
            DestroyImmediate(transform.Find(holderName).gameObject);
        Transform effectHolder = new GameObject(holderName).transform;
        effectHolder.SetParent(transform);

        for (int i = 0; i < Effects.Count; i++)
        {
            Effects[i] = Instantiate(Effects[i], effectHolder);
          
            string stringToRemove = "(Clone)";
            string sourceString = Effects[i].name;
            int index = sourceString.IndexOf(stringToRemove);
            Effects[i].name = (index < 0)
                ? sourceString
                : sourceString.Remove(index, stringToRemove.Length);
        }
    }

    public void UseEffectOnce(string effectName, Vector3 effectPosition)
    {
        GameObject effectToUse = Effects.First(x => x.name == effectName);
        if (effectToUse is null)
            return;

        VisualEffect effectComponent = effectToUse.GetComponent<VisualEffect>();
        effectToUse.transform.position = effectPosition;
        effectComponent.SetVector3("EffectPosition", effectPosition);
        effectComponent.SendEvent("Play");
    }
}