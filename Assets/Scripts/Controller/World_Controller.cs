using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World_Controller : MonoBehaviour
{
    public Sprite GrassSprite,RoadSprite;
     public World World{get;protected set;}
    private void Start() {
        World = new World();
        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                Tile tile_data = World.GetTileAt(x,y);

                GameObject tile_GO = new GameObject();
                tile_GO.name = "Tile_"+x+"_"+y;
                tile_GO.transform.position = new Vector3(tile_data.X,tile_data.Y,0);

                 tile_GO.AddComponent<SpriteRenderer>();
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
    void OnTileTypeChange(Tile tile_data,GameObject tile_go)
    {
        if(tile_data.Type == Tile.TileType.Grass)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = GrassSprite;
        }
        else if(tile_data.Type == Tile.TileType.Road)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = RoadSprite;
        }
        else if(tile_data.Type == Tile.TileType.Empty){
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else{Debug.LogError("OnTileTypeChange - Not Recognized Tile");}
    }
}
