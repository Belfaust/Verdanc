using System.Collections.Generic;
using UnityEngine;

public class World 
{
    Tile[,,] tiles; 
    public int Width{get;protected set;}
    public int Depth{get;protected set;}
    public int Height{get;protected set;}
    public int seed{get;protected set;}
    public World(int width=100,int depth= 100,int height= 2)
    {
        seed = Random.Range(-100000,100000);
        Width = width;
        Depth = depth;
        Height = height;
        tiles = new Tile[this.Width, this.Depth,this.Height];
        float[,] noiseMap = Noise.GenerateNoiseMap(Width,Depth,seed,50,4,.4f,2, new Vector2(10,15));
        for (int x = 0; x < Width; x++)
        { 
            for (int y = 0; y < Depth; y++)
            {   
                int currentHeight = (int)(noiseMap[x,y]*(height*10));
                if(currentHeight>15)
                { currentHeight = 16;}
                for (int z = 0; z < Height; z++)
                {
                tiles[x,y,z] = new Tile(this,x,y,z+currentHeight);     
                }
            }   
        }
        Debug.Log("Created with " +(Width* Depth)+ " tiles");
        BuiltObject wallproto = BuiltObject.CreatePrototype("Wall",1,2,1);
    }
    public void Randomize()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Depth; y++)
            {
                for (int z = 0; z < Height; z++)
                {
                if(tiles[x,y,z].Z <15)
                {
                    tiles[x,y,z].Type = TileType.Grass;
                }
                else
                {
                    tiles[x,y,z].Type = TileType.Water;
                }
                }
            }
        }
        RoadGeneration();
    }  
    // This function can be further upgraded with specific Tile weights However i am gonna leave it as it is for now
    //before making the A* Road there should be a proper world with Elevation based on Perlin Noise
   void RoadGeneration() 
{
List <Tile> Openlist = new List<Tile>();
List <Tile> Closedlist = new List<Tile>();
List <Tile> DestiantionList = new List<Tile>();
Tile CurrentRoadTile= tiles[Random.Range(0,Width),1,0];
int H(int x, int y , int targetX , int targetY)
{
    return Mathf.Abs(targetX-x) + Mathf.Abs(targetY - y);
}
for (int x = 1; x < 4; x++)
{
   DestiantionList.Add(tiles[Random.Range(10,Width-10),25*x,0]);
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
                    Tile tmp_tile = GetTileAt(CurrentTile.X + dx,CurrentTile.Y + dy,0);
                    if(tmp_tile != null&&!NeighbourList.Contains(tmp_tile)&&!ClosedNeighbourList.Contains(tmp_tile)){
                            NeighbourList.Add(tmp_tile);
                    }
                }
            }
        }
}
    public Tile GetTileAt(int x,int y,int z){
        if(x>Width-1||x<0||y>Depth-1||y<0||z>Height-1||z<0)
        {
            Debug.LogError("Tile ("+x+" , "+y+" , " + z+") is out of range.");
            return null;
        }
        return tiles[x, y, z];
    }
}

