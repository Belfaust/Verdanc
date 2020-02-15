using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World_Controller : MonoBehaviour
{
    public static World_Controller _Instance{get;protected set;}
    Dictionary<Tile , GameObject> tileGameobjectMap;
    public Material Grass,Road,Water,Dirt;
    public int Money = 250,Substance = 25;
    public World World{get;protected set;}
 
     void Start() {
        
        if(_Instance != null){
        Debug.Log("Err there are 2 instances of World Controllers");
        }
        else
        {   _Instance = this;}
        CreateNewWorld();   
    }
    public void CreateNewWorld()
    {
        World = new World();                                    //Generating the World with Tile types
        tileGameobjectMap = new Dictionary<Tile, GameObject>(); //Creating the Dictionary to store linked GameObjects and tiles
            for (int x = 0; x < World.Width; x++)               
            {
                for (int y = 0; y < World.Height; y++)
                {
                    for (int z = 0; z < World.Depth; z++)
                    {
                    Tile tile_data = World.GetTileAt(x,y,z);

                    GameObject tile_GO = new GameObject();          // GO is a shortcut for GameObject

                    tileGameobjectMap.Add(tile_data,tile_GO);       //Linking the Gameobject and tile in Dictionary 

                    tile_GO.name = "Tile_"+x+"_"+y+" "+z;
                    tile_GO.transform.position = new Vector3(tile_data.X,tile_data.Y,tile_data.Z);
                    tile_GO.transform.SetParent(this.transform,true);
                    tile_GO.AddComponent<MeshFilter>();
                    tile_GO.SetActive(false);

                        for (int i = 0; i < 6; i++)
                        {
                        if(GetNeighbour(tile_data,(FaceDirections)i) != null&&GetNeighbour(tile_data,(FaceDirections)i).Type == TileType.Empty)
                            {
                            tile_GO.SetActive(true);
                            break;
                            }
                            else if(GetNeighbour(tile_data,(FaceDirections)i) == null)
                            {
                            tile_GO.SetActive(true);
                            break;
                            }
                        }
                    }
            }
        }
        StartCoroutine("GenerateMeshes");
    }
    IEnumerator GenerateMeshes()
    {
        for (int x = 0; x < World.Width; x++)                       //Starting to Generate Visual models for the World
            {
                for (int y = 0; y < World.Height; y++)
                {
                    for (int z = 0; z < World.Depth; z++)
                    {
                        Tile tile_data = World.GetTileAt(x,y,z);
                        OnTileTypeChange(tile_data);                // Executing a callback and adding it to the tile
                        tile_data.RegisterTileTypeChange( OnTileTypeChange );
                    }
                }
            }
        yield return null;
    }
    void OnTileTypeChange(Tile tile_data)
    {
        GameObject tile_GO = tileGameobjectMap[tile_data];
        Mesh tile_mesh = tile_GO.GetComponent<MeshFilter>().mesh;
        if(!tile_GO.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            tile_GO.AddComponent<MeshRenderer>();
        }
        if(tileGameobjectMap.ContainsKey(tile_data)==false)
        {
            Debug.LogError("TileGameobjectMap doesn't contain the tile data");
            return;
        }
        if(tile_GO == null)
        {
            Debug.LogError("TilegameobjectMap's returned Gameobject is null");
            return;
        }
        tile_mesh.Clear();
        TileMeshChange(tile_data,tile_mesh);
        if(tile_data.Type == TileType.Grass)
        {
            tile_GO.GetComponent<MeshRenderer>().material = Grass;
        }
        else if(tile_data.Type == TileType.Road)
        {
            tile_GO.GetComponent<MeshRenderer>().material = Road;
        }
        else if(tile_data.Type == TileType.Water){
            tile_GO.GetComponent<MeshRenderer>().material = Water;
        }
        else if(tile_data.Type == TileType.Dirt){
            tile_GO.GetComponent<MeshRenderer>().material = Dirt;
        }
        else if(tile_data.Type == TileType.Empty){
            tile_mesh.Clear();
            Destroy(tile_GO.GetComponent<MeshRenderer>());
            Destroy(tile_GO.GetComponent<MeshCollider>());
        }
        else{Debug.LogError("OnTileTypeChange - Not Recognized Tile");}
    }
    public void TileMeshChange(Tile tile_data,Mesh tile_mesh)
    {
        bool neighbourscheck = false;                           // checking if theres any neighbours to delete unnecessary colliders
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();                  // Making lists to keep track of the vertices and triangles in this specific mesh
        for (int i = 0; i < 6; i++)                             //Checking all sides of the voxel by changing the int of FaceDirections enum 
        {
            if(GetNeighbour(tile_data,(FaceDirections)i) != null&&GetNeighbour(tile_data,(FaceDirections)i).Type == TileType.Empty)
            {
                neighbourscheck = true;
                MakeFace((FaceDirections)i,tile_data,vertices,triangles);
            }
            else if(GetNeighbour(tile_data,(FaceDirections)i) == null)
            {
                neighbourscheck = true;
                MakeFace((FaceDirections)i,tile_data,vertices,triangles);
            }
        }
        tile_mesh.SetVertices(vertices);
        tile_mesh.triangles = triangles.ToArray(); 
        if(!tileGameobjectMap[tile_data].TryGetComponent<MeshCollider>(out MeshCollider meshCollider)&&neighbourscheck)
        {
        tileGameobjectMap[tile_data].AddComponent<MeshCollider>();
        }
    }
    void MakeFace(FaceDirections dir,Tile tile_data,List<Vector3> vertices,List<int> triangles)
    {   
            vertices.AddRange (CubeMeshData.faceVertices(dir,new Vector3(tile_data.X,tile_data.Y,tile_data.Z)));
            int vCount = vertices.Count;

            triangles.Add(vCount -4);
            triangles.Add(vCount -4 + 1);
            triangles.Add(vCount -4 + 2);
            triangles.Add(vCount -4);
            triangles.Add(vCount -4 + 2);
            triangles.Add(vCount -4 + 3);   
    }
       Vector3[] offsets =                  //Offsets to check the position of Tiles and their GameObjects .Not the position of the vertices
    {
        new Vector3(0,0,1),
        new Vector3(1,0,0),
        new Vector3(0,0,-1),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,-1,0)
    };
    public Tile GetNeighbour(Tile originTile,FaceDirections dir)
    {
        Vector3 offsetToCheck = offsets[(int)dir];
        Vector3 neigbourCoord = new Vector3(originTile.X + offsetToCheck.x,originTile.Y + offsetToCheck.y,originTile.Z + offsetToCheck.z);
        if(neigbourCoord.x>World.Width-1||neigbourCoord.x<0||neigbourCoord.y>World.Height-1||neigbourCoord.y<0||neigbourCoord.z>World.Depth-1||neigbourCoord.z<0)
        {
            return null;
        }
        else
        {
        return GetTileAtWorldCoord(neigbourCoord);
        }
    }
    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);
        int z = Mathf.FloorToInt(coord.z);

        return World.GetTileAt(x , y , z);
    }
}
public enum FaceDirections{
    North,
    East,
    South,
    West,
    Up,
    Down
}
