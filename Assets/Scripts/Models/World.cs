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
                if(Random.Range(0,2)==1)
                {
                    tiles[x,y].Type = Tile.TileType.Grass;
                }
                else
                {
                    tiles[x,y].Type = Tile.TileType.Empty;
                }
            }
        }
        GenerateRoad();
    }
    void GenerateRoad()
    {
        GameObject Road = new GameObject();
        int l = 0;
        int r = Random.Range(0,Width);
        if(tiles[r,0].Type != Tile.TileType.Empty)
        {
            tiles[r,0].Type = Tile.TileType.Road;
        }
        else
        {
            while(tiles[r,0].Type == Tile.TileType.Empty)
            {
                r = Random.Range(0,Width);
                Debug.Log("REshuffles");
            }
                tiles[r,0].Type = Tile.TileType.Road;
        }
        while(l<50)
        {
            int tmpr = Random.Range(0,4);
            
        }
    }
    public Tile GetTileAt(int w,int y){
        if(x>Width||x<0||y>Height||y<0)
        {
            Debug.LogError("Tile ("+x+" , "+y+") is out of range.");
            return null;
        }
        return tiles[x, y];
    }

}
