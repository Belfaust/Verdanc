using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{
    Tile[,] tiles; 
    public int Width{get;protected set;}
    public int Height{get;protected set;}
    PerlinNoise noise;
    public World(int width=100,int height= 100)
    {
        noise = new PerlinNoise(Random.Range(1000000,10000000));
        Width = width;
        Height = height;
        tiles = new Tile[this.Width, this.Height];
        for (int x = 0; x < Width; x++)
        {
            int ColumnHeight = noise.getNoise(x - (-Width),Height -(-Height));
            Debug.Log(ColumnHeight);
            for (int y = 0; y < Height; y++)
            {
                tiles[x,y] = new Tile(this,x,y+ ColumnHeight);     
            }   
        }
        Debug.Log("Created with " +(Width* Height)+ " tiles");
        BuiltObject wallproto = BuiltObject.CreatePrototype("Wall",1,2,1);
    }
    public void Randomize()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if(Random.Range(0,5)%2==0)
                {
                    tiles[x,y].Type = TileType.Grass;
                }
                else
                {
                    tiles[x,y].Type = TileType.Water;
                }
            }
        }
        RoadGeneration();
    }  
    public void PerlinWorldGenerator(Renderer renderer)
    {
        Texture2D GenerateMapTexture()
        {
            Texture2D texture = new Texture2D(256, 256);
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    Color color = CalculateColor(x,y);
                    // = color;
                }
            }
            texture.Apply();
            return texture;
        }
        Color CalculateColor(int x , int y)
        {
            float xCoord = (float)x / 256 * 20;
            float yCoord = (float)y / 256 * 20;


            float sample = Mathf.PerlinNoise(xCoord,yCoord);
            return new Color(sample,sample,sample);
        }       
        renderer.material.mainTexture = GenerateMapTexture();
    }


    // This function can be further upgraded with specific Tile weights However i am gonna leave it as it is for now
    //before making the A* Road there should be A world with Elevation based on Perlin Noise
   void RoadGeneration() 
{
List <Tile> Openlist = new List<Tile>();
List <Tile> Closedlist = new List<Tile>();
List <Tile> DestiantionList = new List<Tile>();
Tile CurrentRoadTile= tiles[Random.Range(0,Width),1];
int H(int x, int y , int targetX , int targetY)
{
    return Mathf.Abs(targetX-x) + Mathf.Abs(targetY - y);
}
for (int x = 1; x < 4; x++)
{
   DestiantionList.Add(tiles[Random.Range(10,Width-10),25*x]);
}
int G = 0,DestinationIndex = 0,HeuristicValue = H(CurrentRoadTile.X,CurrentRoadTile.Y,DestiantionList[DestinationIndex].X,DestiantionList[DestinationIndex].Y);
int F = G + HeuristicValue;
Closedlist.Add(CurrentRoadTile); 
while(CurrentRoadTile!= DestiantionList[DestiantionList.Count-1])
    {
        GetNeighbourTiles(CurrentRoadTile,Openlist,Closedlist);   
        CurrentRoadTile = Openlist[Random.Range(0,Openlist.Count-1)];
        foreach(Tile tile in Openlist)
        { 
            int Local_HeuristicValue = H(tile.X,tile.Y,DestiantionList[DestinationIndex].X,DestiantionList[DestinationIndex].Y);
            G = H(tile.X,tile.Y,Closedlist[0].X,Closedlist[0].Y); 
            if(!Closedlist.Contains(tile))
            {
                if(F > G + Local_HeuristicValue)
                {
                    F = G + Local_HeuristicValue;
                    CurrentRoadTile = tile;  
                
                } else
                {         
                    if(Local_HeuristicValue <= H(CurrentRoadTile.X,CurrentRoadTile.Y,DestiantionList[DestinationIndex].X,DestiantionList[DestinationIndex].Y))
                    {
                        F = G + Local_HeuristicValue;
                        CurrentRoadTile = tile;  
                    }
                }
            }
        }
        Openlist.Remove(CurrentRoadTile);
        Closedlist.Add(CurrentRoadTile);
        if(CurrentRoadTile == DestiantionList[DestinationIndex])
        {
            DestinationIndex+=1;
        }
    }
        foreach(Tile tile in Closedlist)
        {
        tile.Type = TileType.Road;
        }  
}
public void GetNeighbourTiles(Tile CurrentTile,List<Tile> NeighbourList,List<Tile> ClosedNeighbourList)
{
for(int dx = -1; dx <= 1; ++dx) {
            for (int dy = -1; dy <= 1; ++dy) {
                if (dx != 0 || dy != 0) {
                    Tile tmp_tile = GetTileAt(CurrentTile.X + dx,CurrentTile.Y + dy);
                    if(tmp_tile != null&&!NeighbourList.Contains(tmp_tile)&&!ClosedNeighbourList.Contains(tmp_tile)){
                            NeighbourList.Add(tmp_tile);
                    }
                }
            }
        }
}
    public Tile GetTileAt(int x,int y){
        if(x>Width-1||x<0||y>Height-1||y<0)
        {
            Debug.LogError("Tile ("+x+" , "+y+") is out of range.");
            return null;
        }
        return tiles[x, y];
    }
}

