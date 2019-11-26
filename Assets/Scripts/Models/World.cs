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
   void RoadGeneration()
{
List <Tile> openlist = new List<Tile>();
List <Tile> closedlist = new List<Tile>();
List <Tile> Destiantion = new List<Tile>(){tiles[Random.Range(0,Width),Random.Range(0,Height)]};
Tile CurrentRoadTile= tiles[Random.Range(0,Width),1];
int H(int x, int y , int targetX , int targetY)
{
    return Mathf.Abs(targetX-x) + Mathf.Abs(targetY - y);
}
int G = 0,DestinationIndex = 0,HeuristicValue = H(CurrentRoadTile.X,CurrentRoadTile.Y,Destiantion[DestinationIndex].X,Destiantion[DestinationIndex].Y);
int F = G + HeuristicValue;
Debug.Log(" Destination tiles:"+Destiantion[DestinationIndex].X+" "+Destiantion[DestinationIndex].Y+" My Tile"+CurrentRoadTile.X+" "+ CurrentRoadTile.Y);
closedlist.Add(CurrentRoadTile); 
CurrentRoadTile.Type = TileType.Grass;
while(CurrentRoadTile!= Destiantion[DestinationIndex])
    {
        
        for(int dx = -1; dx <= 1; ++dx) {
            for (int dy = -1; dy <= 1; ++dy) {
                if (dx != 0 || dy != 0) {
                    Tile tmp_tile = GetTileAt(CurrentRoadTile.X + dx,CurrentRoadTile.Y + dy);
                    if(tmp_tile != null&&!openlist.Contains(tmp_tile)&&!closedlist.Contains(tmp_tile)){
                        if(tmp_tile.Type != TileType.Road)
                        {
                            openlist.Add(tmp_tile);
                        }
                    }
                }
            }
        }
        CurrentRoadTile = openlist[Random.Range(0,openlist.Count)];
        foreach(Tile tile in openlist)
        { 
            int Local_HeuristicValue = H(tile.X,tile.Y,Destiantion[DestinationIndex].X,Destiantion[DestinationIndex].Y);
            G = H(tile.X,tile.Y,closedlist[0].X,closedlist[0].Y); 
            if(tile.Type == TileType.Water)
            {
                Local_HeuristicValue+=1;
            }
            if(!closedlist.Contains(tile))
            {
                if(F > G + Local_HeuristicValue)
                {
                    F = G + Local_HeuristicValue;
                    CurrentRoadTile = tile;  
                
                } else
                {         
                    if(Local_HeuristicValue <= H(CurrentRoadTile.X,CurrentRoadTile.Y,Destiantion[DestinationIndex].X,Destiantion[DestinationIndex].Y))
                    {
                        F = G + Local_HeuristicValue;
                        CurrentRoadTile = tile;  
                    }
                }
            }
        }
        openlist.Remove(CurrentRoadTile);
        closedlist.Add(CurrentRoadTile);  
        foreach(Tile tile in closedlist)
        {
        tile.Type = TileType.Road;
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

