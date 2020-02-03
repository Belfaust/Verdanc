using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltObject{

    Tile tile;
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

static public BuiltObject PlaceObject(BuiltObject proto, Tile tile)
{
     BuiltObject obj = new BuiltObject();
obj.objectType = proto.objectType;
obj.width = proto.width;
obj.height = proto.height;
obj.depth = proto.depth;

obj.tile = tile;

if(!tile.PlaceObject(obj))
{
  return null;
}
return obj;
}

    
}
