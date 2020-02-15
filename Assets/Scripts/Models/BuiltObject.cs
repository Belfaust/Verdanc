using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltObject{

    Tile[] tile;
    string objectType;
    int width;
    int height;
    int depth;
    int Money_Cost;
    int Substance_Cost;
    

  static public BuiltObject CreatePrototype(string objectType,int width = 1 , int height = 1 , int depth = 1,int Money_Cost = 100, int Substance_Cost = 10)
{
BuiltObject obj = new BuiltObject();
obj.objectType = objectType;
obj.width = width;
obj.height = height;
obj.depth = depth;
obj.Money_Cost = Money_Cost;
obj.Substance_Cost = Substance_Cost;

return obj;
}
static public int[,,] GetSize(BuiltObject Object)
{
  int[,,] Size = new int[Object.width,Object.height,Object.depth];
  return Size;
}
static public int GetMoneyCost(BuiltObject Object)
{
  int MoneyCost = Object.Money_Cost;
  return MoneyCost;
}
static public int GetSubstanceCost(BuiltObject Object)
{
  int Substance_Cost = Object.Substance_Cost;
  return Substance_Cost;
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
    tile[i].PlaceObject(obj);
}
return obj;
}

    
}
