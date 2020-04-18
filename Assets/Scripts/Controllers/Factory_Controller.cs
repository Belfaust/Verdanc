using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Factory_Controller : MonoBehaviour
{
    public Sprite Tree,cursor;
    public Button[] Poaches;
    public GameObject[] Poaches_Positions;
    public Mesh[] Saplings_Models;
    Sapling Sapling_Example = new Sapling();
    private void Awake() {
        Sapling_Example.Fertility = 5;
        Sapling_Example.Growth_Time = 20;
        Sapling_Example.Verdancy = 8;
        for (int i = 0; i < Poaches.Length; i++)
        {
            Sapling IncubatorSapling = Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Factory>().Factory_Sapling_List[i];
            if( IncubatorSapling != null)
            {
                if(IncubatorSapling.Growth_State == 3)
                {
                    Poaches_Positions[i].GetComponent<MeshFilter>().mesh = Saplings_Models[2];
                }
                else if(IncubatorSapling.Growth_State == 2)
                {
                    Poaches_Positions[i].GetComponent<MeshFilter>().mesh = Saplings_Models[1];
                }
                else
                {
                    Poaches_Positions[i].GetComponent<MeshFilter>().mesh = Saplings_Models[0];
                }
            }
        }
    }
    public void World_Map(Button thisButton)
    {
        UI_Controller uI_Controller;
        uI_Controller = GameObject.FindObjectOfType<UI_Controller>();
        thisButton.onClick.AddListener(uI_Controller.World_Map_Button);
        Mouse_Controller._Instance.CursorPrefab.GetComponent<Image>().sprite = Mouse_Controller._Instance.CursorSprite;
        Mouse_Controller._Instance.CurrentlySelectedBuilding = null;
    }
    public void Sapling_Plant_Button()
    {
        Mouse_Controller._Instance.CursorPrefab.GetComponent<Image>().sprite = Tree;
        Mouse_Controller._Instance.CurrentlySelectedSapling = Sapling_Example;
    }
    public void Selling_Sapling(Button thisButton)
    {
        Factory CurrentFactory = Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Factory>();
        Sapling[] FactorySaplingList = CurrentFactory.Factory_Sapling_List;
        for (int i = 0; i < Poaches.Length; i++)
        {
            if(thisButton == Poaches[i])
            {
                Sapling This_Sapling = Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Factory>().Factory_Sapling_List[i];
                if(This_Sapling == null)
                {
                    FactorySaplingList[i] = Mouse_Controller._Instance.CurrentlySelectedSapling;
                    FactorySaplingList[i].Growth_State = 1;
                    Poaches_Positions[i].GetComponent<MeshFilter>().mesh = Sapling_MeshChange(FactorySaplingList[i]);
                    FactorySaplingList[i].Growing = false;
                    FactorySaplingList[i].AddSaplingCB(Sapling_Growth_Update());
                    CurrentFactory.Factory_Sapling_List = FactorySaplingList;
                    FactorySaplingList = null;
                    Mouse_Controller._Instance.CurrentlySelectedSapling = null;
                    Mouse_Controller._Instance.CursorPrefab.GetComponent<Image>().sprite = cursor;
                    
                    break;
                }
                if(This_Sapling.Growth_State == 3&&This_Sapling!=null)
                {
                    Poaches_Positions[i].GetComponent<MeshFilter>().mesh = null;
                    World_Controller._Instance.AddResources((3*(3*This_Sapling.Verdancy)+This_Sapling.Fertility),This_Sapling.Verdancy);
                    Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Factory>().Factory_Sapling_List[i] = null;
                    break;
                }
            }
        }
    }
    public Action Sapling_Growth_Update()
    {
        if(Mouse_Controller._Instance.CurrentlySelectedBuilding!= null)
        {
            for (int i = 0; i < Poaches.Length; i++)
                {   
                    for (int j = 0; j < 3; j++)
                    {
                        if(Mouse_Controller._Instance.CurrentlySelectedBuilding.GetComponent<Factory>().Factory_Sapling_List[i].Growth_State == j)
                        {
                            Poaches_Positions[i].GetComponent<MeshFilter>().mesh = Saplings_Models[j];
                        }
                    }
                }   
        }
        return null;
    }
    private Mesh Sapling_MeshChange(Sapling SelectedSapling)
    {
        Mesh Sapling_mesh = new Mesh();
        if(SelectedSapling.Type == Sapling_Type.Oak)
        {
            Sapling_mesh = Saplings_Models[0];
        }
        else if(SelectedSapling.Type == Sapling_Type.Birch)
        {
            Sapling_mesh = Saplings_Models[0];
        }
        return Sapling_mesh;
    }
}
