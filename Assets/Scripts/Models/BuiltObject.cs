using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltObject{

    Tile tile;

    string objectType;

    float movementCost;

    int width;
    int height;

  static public BuiltObject CreatePrototype(string objectType,float movementCost = 1f,int width =1 , int height =1)
{
    BuiltObject obj = new BuiltObject();
obj.objectType = objectType;
obj.movementCost = movementCost;
obj.width = width;
obj.height = height;

return obj;
}

static public BuiltObject PlaceObject(BuiltObject proto, Tile tile)
{
     BuiltObject obj = new BuiltObject();
obj.objectType = proto.objectType;
obj.movementCost = proto.movementCost;
obj.width = proto.width;
obj.height = proto.height;

obj.tile = tile;

if(!tile.PlaceObject(obj))
{


}
return obj;
}

    
}
