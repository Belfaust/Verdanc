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
Debug.Log(F+"Destination tiles:"+Destiantion[DestinationIndex].X+" "+Destiantion[DestinationIndex].Y+" My Tile"+CurrentRoadTile.X+" "+ CurrentRoadTile.Y);
int length= 0 ;
closedlist.Add(CurrentRoadTile); 

while(CurrentRoadTile!= Destiantion[DestinationIndex]&& length<500)
    {
        for(int dx = -1; dx <= 1; ++dx) {
            for (int dy = -1; dy <= 1; ++dy) {
                if (dx != 0 || dy != 0) {
                    if(GetTileAt(CurrentRoadTile.X + dx,CurrentRoadTile.Y + dy) != null){
                        if(GetTileAt(CurrentRoadTile.X + dx,CurrentRoadTile.Y + dy).Type != TileType.Water && GetTileAt(CurrentRoadTile.X + dx,CurrentRoadTile.Y + dy).Type != TileType.Road)
                        {
                            openlist.Add(GetTileAt(CurrentRoadTile.X + dx,CurrentRoadTile.Y + dy));
                        }
                    }
                }
            }
        }
        foreach(Tile tile in openlist)
        {      
           // Debug.Log(tile.X + " " + tile.Y);
            int Local_Heuristicvalue = H(tile.X,tile.Y,Destiantion[DestinationIndex].X,Destiantion[DestinationIndex].Y);
            G = H(tile.X,tile.Y,closedlist[0].X,closedlist[0].Y); 
            //Debug.Log(G + Local_Heuristicvalue);
            if(F > G + Local_Heuristicvalue)
            {
                F = G + Local_Heuristicvalue;
                CurrentRoadTile = tile;  
                
            } else if(F<= G + Local_Heuristicvalue)
            {
                if(!closedlist.Contains(tile)||Local_Heuristicvalue<= H(CurrentRoadTile.X,CurrentRoadTile.Y,Destiantion[DestinationIndex].X,Destiantion[DestinationIndex].Y))
                {
                        F = G + Local_Heuristicvalue;
                        CurrentRoadTile = tile;  
                }
            }
        }
        length +=1;
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

