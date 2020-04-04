using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Sapling_Type {Birch,Oak,Cherry};
public enum Sapling_Traits {Evolutionary,Gigantism,Mutator,Bushy};
public class Sapling 
{
    Sapling_Type _Type = Sapling_Type.Oak;
    Sapling_Traits[] _Traits = new Sapling_Traits[]; 
    int MyGrowth_State = 0;
    Action GrowthStateChangeCB;
    public Sapling_Type Type{get => _Type;set{}}
   public int Verdancy{get;set;}
   public int Fertility{get;set;}
   public int Growth_Time{get;set;}
   public int Growth_State{get => MyGrowth_State;set{MyGrowth_State = value;if(GrowthStateChangeCB != null)GrowthStateChangeCB();}}
   public bool Growing {get;set;} = false;
    
    public void AddSaplingCB(Action Callback)
    {
        GrowthStateChangeCB += Callback;
    }
}

