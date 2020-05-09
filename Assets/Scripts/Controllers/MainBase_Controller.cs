using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainBase_Controller : MonoBehaviour
{
    MainBase CurrentMainBase = World_Controller._Instance.mainBase;
    public void World_Map(Button thisButton)
    {
        thisButton.onClick.AddListener(UI_Controller._Instance.World_Map_Button);
        Mouse_Controller._Instance.CursorPrefab.GetComponent<Image>().sprite = Mouse_Controller._Instance.CursorSprite;
        Mouse_Controller._Instance.CurrentlySelectedBuilding = null;
    }
    public void SellingSapling()
    {
        
    }
}
