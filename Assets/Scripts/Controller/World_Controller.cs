using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World_Controller : MonoBehaviour
{
    public static World_Controller _Instance{get;protected set;}
    public GameObject GrassTile,RoadTile;
     public World World{get;protected set;}
     void Start() {
        if(_Instance != null){
        Debug.Log("Err there are 2 instances of World Controllers");
        }else{   _Instance = this;}
        World = new World();
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                Tile tile_data = World.GetTileAt(x-World.Width/2,y-World.Height/2);

                GameObject tile_GO = new GameObject();
                tile_GO.name = "Tile_"+x+"_"+y;
                tile_GO.transform.position = new Vector3(tile_data.X,tile_data.Y,0);

                 tile_GO.AddComponent<MeshFilter>();
                 tile_GO.AddComponent<MeshRenderer>();
                 tile_data.RegisterTileTypeChange( (tile) => {OnTileTypeChange(tile,tile_GO);} );
                 tile_GO.transform.SetParent(this.transform,true);
            }
        }
        World.Randomize();
    }
    private void Update() {
      if(Input.GetKeyDown(KeyCode.K))
      {
        World.Randomize();
      }  
    }
    void OnTileTypeChange(Tile tile_data,GameObject tile_GO)
    {
        if(tile_data.Type == Tile.TileType.Grass)
        {
            tile_GO.GetComponent<MeshFilter>().sharedMesh = GrassTile.GetComponent<MeshFilter>().sharedMesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterial = GrassTile.GetComponent<MeshRenderer>().sharedMaterial;
        }
        else if(tile_data.Type == Tile.TileType.Road)
        {
            tile_GO.GetComponent<MeshFilter>().sharedMesh = RoadTile.GetComponent<MeshFilter>().sharedMesh;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterial = RoadTile.GetComponent<MeshRenderer>().sharedMaterial;
        }
        else if(tile_data.Type == Tile.TileType.Empty){
            tile_GO.GetComponent<MeshFilter>().sharedMesh = null;
            tile_GO.GetComponent<MeshRenderer>().sharedMaterial = null;
        }
        else{Debug.LogError("OnTileTypeChange - Not Recognized Tile");}
    }
    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return World.GetTileAt(x , y);
    }
}
