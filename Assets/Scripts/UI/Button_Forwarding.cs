using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Button_Forwarding : MonoBehaviour
{
    UI_Controller uI_Controller;
    Button button;
    private void Awake() {
        uI_Controller = GameObject.FindObjectOfType<UI_Controller>();
        button = this.GetComponent<Button>();
        button.onClick.AddListener(uI_Controller.World_Map_Button);
    }
    public void World_Map()
    {
        
    }
}
