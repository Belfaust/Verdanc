using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Tile 
{
    public enum TileType { Empty,Grass, Road };
    TileType _type = TileType.Empty;  
    BuiltObject builtobject;
    Action<Tile> TileTypeChangeCB;
    World world;
    public TileType Type { get => _type; set{TileType oldType = _type; _type = value; if(TileTypeChangeCB != null && oldType != _type)TileTypeChangeCB(this);} }
    public int X { get;protected set;}
    public int Y { get;protected set;}
    public Tile( World world,int x,int y ) {
        this.world = world;
        this.X = x;
        this.Y = y;
    }
    public void RegisterTileTypeChange(Action<Tile> callback)
    {TileTypeChangeCB += callback;}
    public void UnRegisterTileTypeChange(Action<Tile> callback)
    {TileTypeChangeCB -= callback;}
}
