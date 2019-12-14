using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World_Controller : MonoBehaviour
{
    public static World_Controller _Instance{get;protected set;}
    Dictionary<Tile , GameObject> tileGameobjectMap;
    public GameObject GrassTile,RoadTile,WaterTile,DirtTile;
     public GameObject testCube;
    public World World{get;protected set;}
    public bool autoUpdate;
    List<Vector3> vertices;
    List<int> triangles;
    Vector3[] offsets =
    {
        new Vector3(0,0,1),
        new Vector3(1,0,0),
        new Vector3(0,0,-1),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,-1,0)
    };
 
     void Start() {
        
        if(_Instance != null){
        Debug.Log("Err there are 2 instances of World Controllers");
        }
        else
        {   _Instance = this;}
        CreateNewWorld();   
    }
    private void Update() {
      if(Input.GetKeyDown(KeyCode.K))
      {
        World.Randomize();
      }  
    }
    public void CreateNewWorld()
    {
        World = new World();
        tileGameobjectMap = new Dictionary<Tile, GameObject>();
        
        
            for (int x = 0; x < World.Width; x++)
            {
                for (int y = 0; y < World.Height; y++)
                {
                    for (int z = 0; z < World.Depth; z++)
                    {
                    Tile tile_data = World.GetTileAt(x,y,z);

                    GameObject tile_GO = new GameObject();

                    tileGameobjectMap.Add(tile_data,tile_GO);

                    tile_GO.name = "Tile_"+x+"_"+y+" "+z;
                    tile_GO.transform.position = new Vector3(tile_data.X,tile_data.Y,tile_data.Z);
                    tile_GO.transform.SetParent(this.transform,true);

                    tile_GO.AddComponent<MeshFilter>();
                    tile_GO.AddComponent<MeshRenderer>();

                tile_data.RegisterTileTypeChange( OnTileTypeChange );
                    }
            }
        }
        World.Randomize();
    }
    public void DestroyAllTileGameObjects()
    {
        while(tileGameobjectMap.Count > 0)
        {
            Tile tile_data = tileGameobjectMap.Keys.First();
            GameObject tile_GO = tileGameobjectMap[tile_data];
            tileGameobjectMap.Remove(tile_data);
            tile_data.UnRegisterTileTypeChange(OnTileTypeChange);
                DestroyImmediate(tile_GO);
        }
    }
    void OnTileTypeChange(Tile tile_data)
    {
        if(tileGameobjectMap.ContainsKey(tile_data)==false)
        {
            Debug.LogError("TileGameobjectMap doesn't contain the tile data");
            return;
        }

        GameObject tile_GO = tileGameobjectMap[tile_data];
        if(tile_GO == null)
        {
            Debug.LogError("TilegameobjectMap's returned Gameobject is null");
            return;
        }
        TileMeshChange(tile_data,tile_GO);
        if(tile_data.Type == TileType.Grass)
        {
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = GrassTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Road)
        {
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = RoadTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Water){
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = WaterTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Dirt){
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = DirtTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Empty){
            tile_GO.GetComponent<MeshFilter>().mesh = null;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = null;
        }
        else{Debug.LogError("OnTileTypeChange - Not Recognized Tile");}
        for (int i = 0; i < 6; i++)
            {
                if(GetNeighbour(tile_data,(Direction)i) != null&&GetNeighbour(tile_data,(Direction)i).Type != TileType.Empty)
                {
                tile_GO = tileGameobjectMap[World.GetTileAt(tile_data.X+(int)offsets[i].x,tile_data.Y+(int)offsets[i].y,tile_data.Z+(int)offsets[i].z )];
                TileMeshChange(GetTileAtWorldCoord(new Vector3(tile_data.X,tile_data.Y,tile_data.Z)+ offsets[i]),tile_GO);
                }
            }
    }
    public void TileMeshChange(Tile tile_data,GameObject tile_GO)
    {
        tile_GO.GetComponent<MeshFilter>().mesh = null;
        Mesh mesh = tile_GO.GetComponent<MeshFilter>().mesh;
        Destroy(tile_GO.GetComponent<MeshCollider>());
        
        vertices = new List<Vector3>();
        triangles = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            Debug.Log((Direction)i);
            if(GetNeighbour(tile_data,(Direction)i) != null&&GetNeighbour(tile_data,(Direction)i).Type == TileType.Empty)
            {
                MakeFace((Direction)i,tile_data,tile_GO,vertices,triangles); 
                mesh.vertices = vertices.ToArray();
                mesh.triangles = triangles.ToArray(); 
            }
        }
       tile_GO.AddComponent<MeshCollider>();
    }
    void MakeFace(Direction dir,Tile tile_data,GameObject tile_GO,List<Vector3> vertices,List<int> triangles)
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
    public Tile GetNeighbour(Tile originTile,Direction dir)
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
public enum Direction{
    North,
    East,
    South,
    West,
    Up,
    Down
}
