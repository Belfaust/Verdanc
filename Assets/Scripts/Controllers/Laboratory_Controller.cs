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
    Laboratory CurrentLaboratory;
    void Start()
    {
        CurrentLaboratory = Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Laboratory>();
        SaplingExample = new Sapling();
        SaplingExample.Verdancy = 3;
        SaplingExample.Fertility = 6;
        SaplingExample.Growth_Time = 15;
        SaplingExample.Traits = new Sapling_Traits[1];
        SaplingExample.Traits[0] = Sapling_Traits.Bushy;
        StartCoroutine("ProgressBarCheck");
        UpdatingLaboratoryInfo();
    }
    IEnumerator ProgressBarCheck()
    {
        while(CurrentLaboratory != null)
        {
            for (int i = 0; i < ResearchSlots.Length; i++)
            { 
                ResearchSlots[i].transform.GetComponentInChildren<Scrollbar>().size = CurrentLaboratory.ResearchSlotProgress[i];
            }
            yield return new WaitForSeconds(1);
        }
            yield break;
    }
    public void Research_Slot_Selection(int ResearchSlotIndex)
    {
        CurrentlySelectedResearchSlot = ResearchSlots[ResearchSlotIndex];
    }
    public void Selecting_Sapling(Button SaplingSlot)
    {
        Sapling[] LaboratorySaplingList = CurrentLaboratory.Laboratory_Sapling_List;
        for (int i = 0; i < ResearchSlots.Length; i++)
        {
            if(ResearchSlots[i] == CurrentlySelectedResearchSlot&&LaboratorySaplingList[i] == null)
            {
                   LaboratorySaplingList[i] = SaplingExample;
            }
        }
        UpdatingLaboratoryInfo();
        CurrentLaboratory.Laboratory_Sapling_List = LaboratorySaplingList;
    }
    private void UpdatingLaboratoryInfo()
    {
        for (int i = 0; i < ResearchSlots.Length; i++)
        {
            if(CurrentLaboratory.Laboratory_Sapling_List[i] != null)
            {
             for (int j = 0; j < ResearchSlots[i].transform.childCount; j++)
                    {
                        if(ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text != null&&j==0)
                        {
                        ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = " " + SaplingExample.Type; 
                        }
                        else
                        {
                        Debug.Log("You Don't have a Type Child in first Position in: " + i +" Laboratory Slot" );
                        }
                        if(ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text != null&&j==1)
                        {
                        ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = 
                        "Sapling Type: " + SaplingExample.Type +
                        "\nVerdancy: " + SaplingExample.Verdancy +
                        "\nFertility: " + SaplingExample.Fertility + 
                        "\nGrowthTime: " + SaplingExample.Growth_Time;
                        }else
                        {
                        Debug.Log("You Don't have a Description Child in second Position in: " + i +" Laboratory Slot" );
                        }
                        if(ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text != null&&j==2)
                        {
                        ResearchSlots[i].transform.GetChild(j).GetComponent<TextMeshProUGUI>().text = "Traits: " + SaplingExample.Traits[0]; 
                        break;
                        }
                        else
                        {
                        Debug.Log("You Don't have a Traits Child in third Position in: " + i +" Laboratory Slot" );
                        }
                    }
            }
        }
    }
    public void World_Map(Button thisButton)
    {
        StopCoroutine("ProgressBarCheck");
        thisButton.onClick.AddListener(UI_Controller._Instance.World_Map_Button);
        Mouse_Controller._Instance.CursorPrefab.GetComponent<Image>().sprite = Mouse_Controller._Instance.CursorSprite;
        Mouse_Controller._Instance.CurrentlySelectedBuilding = null;
    }
}
