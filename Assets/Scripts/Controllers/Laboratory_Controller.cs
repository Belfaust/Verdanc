using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Laboratory_Controller : MonoBehaviour
{
    public GameObject[] ResearchSlots;
    private GameObject CurrentlySelectedResearchSlot;
    Sapling SaplingExample;
    void Start()
    {
        SaplingExample = new Sapling();
        SaplingExample.Verdancy = 3;
        SaplingExample.Fertility = 6;
        SaplingExample.Growth_Time = 15;
        SaplingExample.Traits = new Sapling_Traits[1];
        SaplingExample.Traits[0] = Sapling_Traits.Bushy;

    }
    IEnumerator ProgressBarCheck()
    {
        if(Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Laboratory>() != null)
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < ResearchSlots.Length; i++)
            {    
              //  ResearchSlots[i].transform.GetComponentInChildren(typeof(Scrollbar),false).value = Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Laboratory>().ResearchSlotProgress[i];
            }
        }
        else
        {
            yield break;
        }
    }
    public void Research_Slot_Selection(int ResearchSlotIndex)
    {
        CurrentlySelectedResearchSlot = ResearchSlots[ResearchSlotIndex];
    }
    public void Selecting_Sapling(Button SaplingSlot)
    {
        Laboratory CurrentLaboratory = Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Laboratory>();
        Sapling[] LaboratorySaplingList = CurrentLaboratory.Laboratory_Sapling_List;
        for (int i = 0; i < ResearchSlots.Length; i++)
        {
            if(ResearchSlots[i] == CurrentlySelectedResearchSlot&&SaplingExample.UnderResearch == false)
            {
                    for (int j = 0; j < ResearchSlots[i].transform.childCount; j++)
                    {
                        if(ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text != null&&j==0)
                        {
                        ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = " " + SaplingExample.Type; 
                        }
                        if(ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text != null&&j==1)
                        {
                        ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = 
                        "Sapling Type: " + SaplingExample.Type +
                        "\nVerdancy: " + SaplingExample.Verdancy +
                        "\nFertility: " + SaplingExample.Fertility + 
                        "\nGrowthTime: " + SaplingExample.Growth_Time;
                         
                        }
                        if(ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text != null&&j==2)
                        {
                        ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = "Traits: " + SaplingExample.Traits[0]; 
                        break;
                        }
                    }
                    LaboratorySaplingList[i] = SaplingExample;
            }
        }
        CurrentLaboratory.Laboratory_Sapling_List = LaboratorySaplingList;
    }
    public void World_Map(Button thisButton)
    {
        UI_Controller uI_Controller;
        uI_Controller = GameObject.FindObjectOfType<UI_Controller>();
        thisButton.onClick.AddListener(uI_Controller.World_Map_Button);
        Mouse_Controller._Instance.CursorPrefab.GetComponent<Image>().sprite = Mouse_Controller._Instance.CursorSprite;
        Mouse_Controller._Instance.CurrentlySelectedBuilding = null;
    }
}
