using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World_Controller : MonoBehaviour
{
    public static World_Controller _Instance{get;protected set;}
    Dictionary<Tile , GameObject> tileGameobjectMap;
    public GameObject GrassTile,RoadTile,WaterTile,DirtTile;
    public World World{get;protected set;}
    public bool autoUpdate;
       
    public int seed;
     void Start() {
        seed = Random.Range(-100000,100000);
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
        float[,] noiseMap = Noise.GenerateNoiseMap(World.Width,World.Height,seed,50,4,.5f,2, new Vector2(10,15));
        for (int z = 0; z < World.Depth; z++)
        {
            for (int x = 0; x < World.Width; x++)
            {
                for (int y = 0; y < World.Height; y++)
                {
                int currentHeight = (int)(noiseMap[x,y]*(World.Depth*10));
                if(currentHeight>15)
                    {
                        currentHeight = 15;
                    }

                Tile tile_data = World.GetTileAt(x,y,z);

                GameObject tile_GO = new GameObject();

                tileGameobjectMap.Add(tile_data,tile_GO);

                tile_GO.name = "Tile_"+x+"_"+y+" "+z;
                tile_GO.transform.position = new Vector3(tile_data.X,tile_data.Y,tile_data.Z+currentHeight);
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

        if(tile_data.Type == TileType.Grass)
        {
            tile_GO.GetComponent<MeshFilter>().mesh = GrassTile.GetComponent<MeshFilter>().mesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = GrassTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Road)
        {
            tile_GO.GetComponent<MeshFilter>().mesh = RoadTile.GetComponent<MeshFilter>().mesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = RoadTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Water){
            tile_GO.GetComponent<MeshFilter>().mesh = WaterTile.GetComponent<MeshFilter>().mesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = WaterTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Dirt){
            tile_GO.GetComponent<MeshFilter>().mesh = DirtTile.GetComponent<MeshFilter>().mesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = DirtTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Empty){
            tile_GO.GetComponent<MeshFilter>().mesh = null;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = null;
        }
        else{Debug.LogError("OnTileTypeChange - Not Recognized Tile");}
            OnTileMeshChange(tile_data,tile_GO);
    }
    public void OnTileMeshChange(Tile tile_data,GameObject tile_GO)
    {
       CubeTileOptimization(0,6,tile_data,tile_GO,0,0,1);
    //  CubeTileOptimization(6,0,tile_data,tile_GO,0,0,-1);
    //    CubeTileOptimization(0,6,tile_data,tile_GO,0,1,0);
    //    CubeTileOptimization(0,6,tile_data,tile_GO,0,-1,0);
    //   CubeTileOptimization(6,0,tile_data,tile_GO,1,0,0);
    //    CubeTileOptimization(0,6,tile_data,tile_GO,-1,0,0);
    }
    void CubeTileOptimization(int i,int j,Tile tile_data,GameObject tile_GO,int xoffset,int yoffset,int zoffset)
    {
        if(GetTileAtWorldCoord(new Vector3(tile_data.X+xoffset,tile_data.Y+yoffset,tile_data.Z+zoffset)) != null)
                {
                    Destroy(tile_GO.GetComponent<MeshCollider>());
                    Mesh mesh = tile_GO.GetComponent<MeshFilter>().mesh;
                    int[] oldTriangles = mesh.triangles;
                    int[] newTriangles =  new int[mesh.triangles.Length];
                    int g = 0;
                    while(j < mesh.triangles.Length)
                    {
                         if(g != i)
                         {
                            
                            newTriangles[g++] = oldTriangles[j++];
                            newTriangles[g++] = oldTriangles[j++];
                            newTriangles[g++] = oldTriangles[j++]; 
                         }
                         else
                         {
                                g+=6;
                         }
                    }
                    mesh.triangles = newTriangles;
                    tile_GO.AddComponent<MeshCollider>();
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
