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
    public GameObject Main_UI;

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
        TimeText.text = "0";
    }
    void UnloadAllScenes(string sceneName)
    {
        int SceneCount = SceneManager.sceneCount;
        for (int i = 0; i < SceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
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
        UI_Controller._Instance.TimeText.text = "Day :"+ World_Controller._Instance.WorldTime; 
    }
    public void World_Map_Button()
    {
        World_Controller._Instance.OnWorldMap = true;
        UnloadAllScenes("WorldScene");
        Main_UI.SetActive(true);
    }
    public static void LoadFactory()
    {        
        SceneManager.LoadScene("Factory",LoadSceneMode.Additive);    
        UI_Controller._Instance.Main_UI.SetActive(false);
    }
    public static void LoadLaboratory()
    {
        SceneManager.LoadScene("Laboratory",LoadSceneMode.Additive);    
        UI_Controller._Instance.Main_UI.SetActive(false);
    }
    public static void LoadMainBase()
    {
        SceneManager.LoadScene("MainBase",LoadSceneMode.Additive);    
        UI_Controller._Instance.Main_UI.SetActive(false);
    }
    

}
