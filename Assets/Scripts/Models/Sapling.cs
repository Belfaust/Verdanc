using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sapling_Type {Birch,Oak,Cherry};
public class Sapling : Item
{
    Sapling_Type _Type = Sapling_Type.Oak;
    public Sapling_Type Type{get => _Type;set{}}
   public int Verdancy{get;protected set;}
   public int Fertility{get;protected set;}
   public int Growth_Time{get;protected set;}
   public int Growth_State{get;set;}
}
