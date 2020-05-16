using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Sapling_Type {Birch,Oak,Cherry};
public enum Sapling_Traits {Evolutionary,Gigantism,Mutator,Bushy};
public class Sapling 
{
    Sapling_Type _Type = Sapling_Type.Oak;
    int MyGrowth_State = 0;
    Action GrowthStateChangeCB;
    public Sapling_Type Type{get => _Type;set{_Type = value;Money_Base_Type_Check();}}
    public Sapling_Traits[] Traits;
    public string name{get;set;}
   public int Verdancy{get;set;}
   public int Fertility{get;set;}
   public int Growth_Time{get;set;}
   public float Money_Base;
   public int Growth_State{get => MyGrowth_State;set{MyGrowth_State = value;if(GrowthStateChangeCB != null)GrowthStateChangeCB();}}
   public bool Growing {get;set;} = false;
   public bool UnderResearch {get;set;} = false;
    
    public void AddSaplingCB(Action Callback)
    {
        GrowthStateChangeCB += Callback;
    }
    public void Money_Base_Type_Check()
    {
        if(_Type == Sapling_Type.Oak)
        {
            Money_Base = 3;
        }
        else if(_Type == Sapling_Type.Birch)
        {
            Money_Base = 2;
        }
        else if(_Type == Sapling_Type.Cherry)
        {
            Money_Base = 4;
        }
    }
}

