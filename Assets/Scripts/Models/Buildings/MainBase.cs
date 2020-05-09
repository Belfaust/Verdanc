using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : MonoBehaviour
{
    public List<Sapling> InventorySaplingList;
    public void Selling(Sapling Saplingexample)
    {
        for (int i = 0; i < InventorySaplingList.Count; i++)
        {
            if (InventorySaplingList[i] == Saplingexample&& InventorySaplingList[i].Growth_State == 3)
            {
            World_Controller._Instance.AddResources((int)(Saplingexample.Money_Base * (Saplingexample.Verdancy * 3) + Saplingexample.Fertility),0);
            Saplingexample = null;       
            }
        }
    }
}
