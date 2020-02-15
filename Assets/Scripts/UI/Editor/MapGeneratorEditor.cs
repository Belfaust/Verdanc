using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Generator))]
public class MapGeneratorEditor : Editor
{
 public override void OnInspectorGUI()
 {
     Generator mapGen = (Generator)target;
    DrawDefaultInspector();

    if(DrawDefaultInspector())
    {
        if(mapGen.autoUpdate)
        {
            mapGen.GenerateMap();
        }
    }



    if(GUILayout.Button("Generate"))
    {
        mapGen.GenerateMap();
    }


 }
}
