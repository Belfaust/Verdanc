using System.Collections.Generic;
using UnityEngine;

public class World 
{
    Tile[,,] tiles; 
    public int Width{get;protected set;}
    public int Height{get;protected set;}
    public int Depth{get;protected set;}
    public int seed{get;protected set;}
    public int ChunkSize{get;protected set;}
    public World(int chunkSize = 8)
    {
        ChunkSize = chunkSize;
        Width = chunkSize * 4;
        Height = chunkSize * 4;
        Depth = chunkSize;
        seed = Random.Range(-100000,100000);
        tiles = new Tile[this.Width, this.Height,this.Depth];
        float[,] noiseMap = Noise.GenerateNoiseMap(Width,Height,seed,50,2,.5f,2, new Vector2(5,8));
        for (int x = 0; x < Width; x++)
        { 
            for (int y = 0; y < Height; y++)
            {
                for (int z = 0; z < Depth; z++)
                {   
                        tiles[x,y,z] = new Tile(this,x,y,z);     
                }
                    int currentHeight = (int)(noiseMap[x,y]*(Depth-5));
                    for (int z = 1; z < Depth-1; z++)
                {
                if(tiles[x,y,z].Z == currentHeight)
                {
                    tiles[x,y,z].Type = TileType.Grass;
                }
                if(tiles[x,y,z].Z >currentHeight&&tiles[x,y,z].Z<5)
                {
                    tiles[x,y,z].Type = TileType.Water;
                }
                if(GetTileAt(x,y,z).Z != currentHeight&&GetTileAt(x,y,z).Z<currentHeight)
                {
                    tiles[x,y,z].Type = TileType.Dirt;
                }
                }
            } 
        }
        Debug.Log("Created with " +(Width* Height)+ " tiles");
        //RoadGeneration(noiseMap);  
    }   
    // This function can be further upgraded with specific Tile weights However i am gonna leave it as it is for now
    //before making the A* Road there should be a proper world with Elevation based on Perlin Noise
   void RoadGeneration(float[,] noiseMap) 
{
List <Tile> Openlist = new List<Tile>();
List <Tile> Closedlist = new List<Tile>();
List <Tile> DestiantionList = new List<Tile>();
int RandomPos = Random.Range(0,Width); Tile CurrentRoadTile= tiles[RandomPos,1,(int)(noiseMap[RandomPos,1]*(Depth-5))];
int H(Vector3 OriginalPosition , Vector3 TargetPosition)
{
    return (int)(Mathf.Abs(TargetPosition.x -OriginalPosition.x) + Mathf.Abs(TargetPosition.y - OriginalPosition.y) + Mathf.Abs(TargetPosition.z - OriginalPosition.z));
}
for (int x = 1; x < 4; x++)
{

   RandomPos = Random.Range(10,Width-10);
   DestiantionList.Add(tiles[RandomPos,15*x,(int)(noiseMap[RandomPos,10*x]*(Depth-5))+1]);
}
int G = 0;
int DestinationIndex = 0;
int HeuristicValue = H(new Vector3(CurrentRoadTile.X,CurrentRoadTile.Y,CurrentRoadTile.Z),new Vector3(DestiantionList[DestinationIndex].X,DestiantionList[DestinationIndex].Y,DestiantionList[DestinationIndex].Z));
int F = G + HeuristicValue;
Closedlist.Add(CurrentRoadTile); 
while(CurrentRoadTile!= DestiantionList[DestiantionList.Count-1])
    {
        int a = 0;
        GetNeighbourTiles(CurrentRoadTile,Openlist,Closedlist);   
        CurrentRoadTile = Openlist[Random.Range(0,Openlist.Count-1)];
        foreach(Tile tile in Openlist)
        { 
            int Local_HeuristicValue = H(new Vector3(tile.X,tile.Y,tile.Z),new Vector3(DestiantionList[DestinationIndex].X,DestiantionList[DestinationIndex].Y,DestiantionList[DestinationIndex].Z));
            G = H(new Vector3(tile.X,tile.Y,tile.Z),new Vector3(Closedlist[0].X,Closedlist[0].Y,Closedlist[0].Z)); 
            if(!Closedlist.Contains(tile))
            {
                if(F > G + Local_HeuristicValue)
                {
                    F = G + Local_HeuristicValue;
                    CurrentRoadTile = tile;  
                
                } else
                {         
                    if(Local_HeuristicValue <= H(new Vector3(CurrentRoadTile.X,CurrentRoadTile.Y,CurrentRoadTile.Z),new Vector3(DestiantionList[DestinationIndex].X,DestiantionList[DestinationIndex].Y,DestiantionList[DestinationIndex].Z)))
                    {
                        F = G + Local_HeuristicValue;
                        CurrentRoadTile = tile;  
                    }
                }
            }
            a+=1;
        }
        if(a>1000)
        {
            break;
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
                for (int dz = 0; dz <= 1; dz++)
                {
                if (dx != 0 || dy != 0 || dz != 0) {
                    Tile tmp_tile = GetTileAt(CurrentTile.X + dx,CurrentTile.Y + dy,CurrentTile.Z +dz);
                    if(tmp_tile != null&&!NeighbourList.Contains(tmp_tile)&&!ClosedNeighbourList.Contains(tmp_tile)){
                            NeighbourList.Add(tmp_tile);
                        }
                    }
                }
            }
        }
}
    public Tile GetTileAt(int x,int y,int z){
        if(x>Width-1||x<0||y>Height-1||y<0||z>Depth-1||z<0)
        {
            Debug.LogError("Tile ("+x+" , "+y+" , "+z+") is out of range.");
            return null;
        }
        return tiles[x, y, z];
    }
}

