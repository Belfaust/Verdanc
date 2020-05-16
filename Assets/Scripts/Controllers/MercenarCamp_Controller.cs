using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenarCamp_Controller : MonoBehaviour
{
    MErcenaryCamp CurrentMercenaryCamp;
    GameObject MapMesh;
    private void Start() {
        CurrentMercenaryCamp = Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<MErcenaryCamp>();
        MapMesh = new GameObject();
        MapMesh.transform.position = new Vector3(transform.position.x-10,transform.position.y-10,transform.position.z +10 );
        MapMesh.AddComponent<MeshFilter>();
        MapMesh.AddComponent<MeshRenderer>();
        GenerateMap();
    }
    void GenerateMap()
    {
        List<Vector3> MapVertices = new List<Vector3>(MapMesh.GetComponent<MeshFilter>().mesh.vertices);
        List<int> MapTriangles = new List<int>(MapMesh.GetComponent<MeshFilter>().mesh.triangles);
        for (int y = 0; y < CurrentMercenaryCamp.Height; y++)
        {
            for (int x = 0; x < CurrentMercenaryCamp.Width; x++)
            {
                MakeNodeFace(MapTriangles,MapVertices,CurrentMercenaryCamp.MapNodes[x,y]);
            }
        }
        MapMesh.GetComponent<MeshFilter>().mesh.SetVertices(MapVertices);
        MapMesh.GetComponent<MeshFilter>().mesh.SetTriangles(MapTriangles.ToArray(),0);
    }
    void MakeNodeFace(List<int> triangles,List<Vector3> vertices,Node CurrentNode)
    {
            vertices.Add(new Vector3(CurrentNode.x+1,CurrentNode.y,0));
            vertices.Add(new Vector3(CurrentNode.x,CurrentNode.y,0));
            vertices.Add(new Vector3(CurrentNode.x,CurrentNode.y+1,0));
            vertices.Add(new Vector3(CurrentNode.x+1,CurrentNode.y+1,0));

            int vCount = vertices.Count;
            triangles.Add(vCount -4);
            triangles.Add(vCount -4 + 1);
            triangles.Add(vCount -4 + 2);
            triangles.Add(vCount -4);
            triangles.Add(vCount -4 + 2);
            triangles.Add(vCount -4 + 3);
    }
}
