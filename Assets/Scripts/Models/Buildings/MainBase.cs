﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : MonoBehaviour
{
    public List<Sapling> InventorySaplingList = new List<Sapling>();
    public List<Sapling> SaplingListToSell = new List<Sapling>();
    public void Selling(Sapling Saplingexample)
    {
        for (int i = 0; i < InventorySaplingList.Count; i++)
        {
            if (InventorySaplingList[i] == Saplingexample&& InventorySaplingList[i].Growth_State == 3)
            {
                SaplingListToSell.Add(Saplingexample);
                Saplingexample = null;       
            }
        }
    }
    public void SendingSaplings()
    {
        for (int i = 0; i < SaplingListToSell.Count; i++)
        {
            World_Controller._Instance.AddResources((int)(SaplingListToSell[i].Money_Base*SaplingListToSell[i].Verdancy*3) + SaplingListToSell[i].Fertility,0);
        }
        SaplingListToSell = new List<Sapling>();
    }
}