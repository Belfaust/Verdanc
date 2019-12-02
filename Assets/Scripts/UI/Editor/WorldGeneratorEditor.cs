using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (World_Controller))]
public class WorldGeneratorEditor : Editor
{
   public override void OnInspectorGUI()
 {
     World_Controller WorldGen = (World_Controller)target;
    DrawDefaultInspector();

    if(DrawDefaultInspector())
    {
        if(WorldGen.autoUpdate)
        {
            WorldGen.World.Randomize();
        }
    }



    if(GUILayout.Button("Generate"))
    {
       WorldGen.CreateNewWorld();
    }
    if(GUILayout.Button("Clean World"))
    {
       WorldGen.DestroyAllTileGameObjects();
    }


 }
}
