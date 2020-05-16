using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainBase_Controller : MonoBehaviour
{
    MainBase CurrentMainBase = World_Controller._Instance.mainBase;
    Sapling SelectedSapling;
    public TextMeshProUGUI ListText;
    Sapling SaplingExample = new Sapling();
    void Start()
    {
        SaplingExample.name = "Example Sapling";
        SaplingExample.Verdancy = 3;
        SaplingExample.Fertility = 6;
        SaplingExample.Growth_Time = 15;
        SaplingExample.Traits = new Sapling_Traits[1];
        SaplingExample.Growth_State =3;
        SaplingExample.Traits[0] = Sapling_Traits.Bushy;
        CurrentMainBase.InventorySaplingList.Add(SaplingExample);
    }
    public void World_Map(Button thisButton)
    {
        thisButton.onClick.AddListener(UI_Controller._Instance.World_Map_Button);
        Mouse_Controller._Instance.CursorPrefab.GetComponent<Image>().sprite = Mouse_Controller._Instance.CursorSprite;
        Mouse_Controller._Instance.CurrentlySelectedBuilding = null;
    }
    public void SelectingSaplingFromInventory()
    {
        SelectedSapling = SaplingExample;
    }
    public void SellingSapling()
    {
        CurrentMainBase.PuttingSaplingOnList(SelectedSapling);
        UpdateShippingList(SelectedSapling.name);
    }
    void UpdateShippingList(string textToAdd)
    {
        ListText.text = " ";
        for (int i = 0; i < CurrentMainBase.SaplingListToSell.Count; i++)
        {
            ListText.text = ListText.text + ("\n" + CurrentMainBase.SaplingListToSell[i].name);    
        }
        
    }
}
