using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{
    Tile[,] tiles; 
    public int Width{get;protected set;}
    public int Height{get;protected set;}
    public World(int width=100,int height= 100)
    {
        Width = width;
        Height = height;
        tiles = new Tile[this.Width, this.Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                tiles[x,y] = new Tile(this,x,y);        
            }   
        }
        Debug.Log("Created with " +(Width* Height)+ " tiles");
    }
    public void Randomize()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if(Random.Range(0,2)==0)
                {
                    tiles[x,y].Type = Tile.TileType.Grass;
                }
                else if(Random.Range(0,2) == 1) {
                    tiles[x,y].Type = Tile.TileType.Road;
                }
                else{
                    tiles[x,y].Type = Tile.TileType.Empty;
                }
            }
        }
    }
    public Tile GetTileAt(int x,int y){
        if(x>Width||x<0||y>Height||y<0)
        {
            Debug.LogError("Tile ("+x+" , "+y+") is out of range.");
            return null;
        }
        return tiles[x, y];
    }

}
