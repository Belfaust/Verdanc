using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltObject{

    Tile[] tile;
    string objectType;
    int width;
    int height;
    int depth;
    

  static public BuiltObject CreatePrototype(string objectType,int width = 1 , int height = 1 , int depth = 1)
{
BuiltObject obj = new BuiltObject();
obj.objectType = objectType;
obj.width = width;
obj.height = height;
obj.depth = depth;
return obj;
}
static public int[,,] GetSize(BuiltObject Object)
{
  int[,,] Size = new int[Object.width,Object.height,Object.depth];
  return Size;
}
static public BuiltObject PlaceObject(BuiltObject proto, Tile[] tile)
{
  BuiltObject obj = new BuiltObject();
obj.objectType = proto.objectType;
obj.width = proto.width;
obj.height = proto.height;
obj.depth = proto.depth;
 
obj.tile = tile;
for (int i = 0; i < tile.Length; i++)
{
    tile[i].Type = TileType.Road;
    tile[i].PlaceObject(obj);
}
return obj;
}

    
}
