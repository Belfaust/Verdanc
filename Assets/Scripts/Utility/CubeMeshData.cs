using System.Collections;
using UnityEngine;

public static class CubeMeshData 
{
    public static Vector3[] vertices = {
        new Vector3( 1, 1, 1),
        new Vector3( 0, 1, 1),
        new Vector3( 0, 0, 1),
        new Vector3( 1, 0, 1),
        new Vector3( 0, 1, 0),
        new Vector3( 1, 1, 0),
        new Vector3( 1, 0, 0),
        new Vector3( 0, 0, 0),
    };
    public static int[][] facetriangles ={
        new int[] {0,1,2,3}, // East face of the cube
        new int[] {5,0,3,6}, // North face of the cube
        new int[] {4,5,6,7}, // West face of the cube
        new int[] {1,4,7,2}, // South face of the cube
        new int[] {5,4,1,0}, // Upper face of the cube
        new int[] {3,2,7,6}  // Lower face of the cube
    };
    public static Vector3[] faceVertices(int dir,Vector3 pos)
    {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++)
        {
            fv[i] = vertices[facetriangles[dir][i]] + pos;
        }
        return fv;
    }
    public static Vector3[] faceVertices(FaceDirections dir,Vector3 pos)
    {
        return faceVertices((int)dir,pos);
    }
}
