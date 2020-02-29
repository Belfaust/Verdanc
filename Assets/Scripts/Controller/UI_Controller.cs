using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Controller : MonoBehaviour
{
    public static UI_Controller _Instance{get;protected set;}
    public TextMeshProUGUI TimeText,MoneyText,SubstanceText;
    private void Start() {
         if(_Instance != null){
        Debug.Log("Err there are 2 instances of UI Controllers");
        }
        else
        {   _Instance = this;}
        UpdateResources();
    }
    public void UpdateResources()
    {
        MoneyText.text = "Money: " + World_Controller._Instance.Money;
        SubstanceText.text = "Substances: " + World_Controller._Instance.Substance;
    }
    public void UpdateTime()
    {
        UI_Controller._Instance.TimeText.text = "Day :"+ World_Controller._Instance.Time/5; 
    }

}
