using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class UI_Controller : MonoBehaviour
{
    public static UI_Controller _Instance{get;protected set;}
    public TextMeshProUGUI TimeText,MoneyText,SubstanceText;
    public Canvas Main_UI;
    public string Factory_Scene_Name;

    private void Start() {
         if(_Instance != null){
        Debug.Log("Err there are 2 instances of UI Controllers");
        Destroy(this);
        }
        else
        {
        _Instance = this;
        DontDestroyOnLoad(this.gameObject);
        }
        UpdateResources();
    }
    void UnloadAllScenes(string sceneName)
    {
        int SceneCount = SceneManager.sceneCount;
        for (int i = 0; i < SceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            print(scene.name);
            if(scene.name != sceneName)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

    }
    public void UpdateResources()
    {
        MoneyText.text = "Money: " + World_Controller._Instance.Money;
        SubstanceText.text = "Substances: " + World_Controller._Instance.Substance;
    }
    public void UpdateTime()
    {
        UI_Controller._Instance.TimeText.text = "Day :"+ World_Controller._Instance.Time; 
    }
    public void World_Map_Button()
    {
        World_Controller._Instance.OnWorldMap = true;
        UnloadAllScenes("WorldScene");
        Main_UI.gameObject.SetActive(true);
     }
    public void LoadFactory(Factory factory)
    {        
        SceneManager.LoadScene("Factory",LoadSceneMode.Additive);    
        Main_UI.gameObject.SetActive(false);
    }

}
