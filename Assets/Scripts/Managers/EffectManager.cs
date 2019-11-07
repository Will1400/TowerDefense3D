using UnityEngine;
using System.Collections;
using OdinSerializer;
using System.Collections.Generic;

public class EffectManager : SerializedMonoBehaviour
{
    public UnitySerializedDictionary<string, GameObject> Effects;
}