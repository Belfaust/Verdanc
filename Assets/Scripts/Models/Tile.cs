using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
   public enum TileType { Empty,Grass,Road,Water,Dirt };
public class Tile 
{
 
    TileType _type = TileType.Empty;  
    public BuiltObject builtobject;
    Action<Tile> TileTypeChangeCB;
    World world;
    public TileType Type { get => _type; set{TileType oldType = _type; _type = value; if(TileTypeChangeCB != null && oldType != _type)TileTypeChangeCB(this);} }
    public int X { get;protected set;}
    public int Y { get;protected set;}
    public int Z { get;protected set;}
    public Tile( World world,int x,int y,int z) {
        this.world = world;
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    public void RegisterTileTypeChange(Action<Tile> callback)
    {TileTypeChangeCB += callback;}
    public void UnRegisterTileTypeChange(Action<Tile> callback)
    {TileTypeChangeCB -= callback;}
    public bool PlaceObject(BuiltObject objInstance)
    {
        if(objInstance == null)
        {
            builtobject = null;
            return true;
        }
        if(builtobject != null)
        {
            Debug.LogError("You tried to instal object when you already have one");
            return false;
        }

        builtobject = objInstance;
        return true;
    }
}
