using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class World_Controller : MonoBehaviour
{
    public static World_Controller _Instance{get;protected set;}
    Dictionary<Tile , GameObject> tileGameobjectMap;
    public Renderer PerlinTest;
    public GameObject GrassTile,RoadTile,WaterTile,DirtTile;
    public World World{get;protected set;}
    public bool autoUpdate;
       
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
        //World.PerlinWorldGenerator(PerlinTest);
      }  
    }
    public void CreateNewWorld()
    {
        World = new World();
        tileGameobjectMap = new Dictionary<Tile, GameObject>();
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Depth; y++)
            {
               
                for (int z = 0; z < World.Height; z++)
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

        if(tile_data.Type == TileType.Grass)
        {
            tile_GO.GetComponent<MeshFilter>().sharedMesh = GrassTile.GetComponent<MeshFilter>().sharedMesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = GrassTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Road)
        {
            tile_GO.GetComponent<MeshFilter>().sharedMesh = RoadTile.GetComponent<MeshFilter>().sharedMesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = RoadTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Water){
            tile_GO.GetComponent<MeshFilter>().sharedMesh = WaterTile.GetComponent<MeshFilter>().sharedMesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = WaterTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Dirt){
            tile_GO.GetComponent<MeshFilter>().sharedMesh = DirtTile.GetComponent<MeshFilter>().sharedMesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = DirtTile.GetComponent<MeshRenderer>().sharedMaterials;
        }
        else if(tile_data.Type == TileType.Empty){
            tile_GO.GetComponent<MeshFilter>().sharedMesh = null;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterials = null;
        }
        else{Debug.LogError("OnTileTypeChange - Not Recognized Tile");}
    }
    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);
        int z = Mathf.FloorToInt(coord.z);

        return World.GetTileAt(x , y , z);
    }
}
