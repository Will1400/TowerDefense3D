using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Map"))
        {
            MapGenerator map = target as MapGenerator;
            map.GenerateMap();
            //map.RenderMap();
            map.StartCoroutine(map.RenderMap());
        }
    }
}