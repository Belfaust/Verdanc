﻿using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Mouse_Controller : MonoBehaviour
{
    public static Mouse_Controller _Instance{get;protected set;}
    public GameObject CursorPrefab,FactoryModel,MainBaseModel,LaboratoryModel,CurrentlySelectedBuilding;
    public Sprite CursorSprite;
    public AnimationClip MainBaseLanding;
    private Vector3 CurrentFramePos = new Vector3(-.5f,-.5f);
    private Vector3 Last_Frame_Pos,NotOffsetCamera,TileStartDragPos;
    public Sapling CurrentlySelectedSapling;
    public float offset_z = 2;
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
     Ray ray = Camera.main.ScreenPointToRay(screenPosition);
     Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
     float distance;
     xy.Raycast(ray, out distance);    
     return ray.GetPoint(distance);
 }  
    private void Awake() {
         if(_Instance != null){
        Debug.Log("Err there are 2 instances of Mouse Controllers");
        Destroy(this);
        }
        else
        {   _Instance = this;
        DontDestroyOnLoad(this.gameObject);
        }
        BuildingPreview = new GameObject();
    }
     void Update() 
    {
        UpdateCursor();   
        if(World_Controller._Instance.OnWorldMap == true)
        {
        CurrentFramePos = new Vector3(GetWorldPositionOnPlane(Input.mousePosition,0).x +.5f,GetWorldPositionOnPlane(Input.mousePosition,0).y +.5f); // this is the Camera set up for Dealing with tiles since there is no better way to offset it that i know of
        NotOffsetCamera = GetWorldPositionOnPlane(Input.mousePosition,0);// True Camera position on a plane that is generated in front of it to create a smooth transition of moving
        CameraMovement();    
        Building();
        BuildingSelection();
        }
    }
    void UpdateCursor()
    {        
        Vector3 MousePositionOnPlane = new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z);
        CursorPrefab.transform.position = MousePositionOnPlane;
    }
    void UpdateDragging()
    {
    TileType SelectedBuildTiles = TileType.Road;
    List<GameObject> dragPreviewObjects = new List<GameObject>();    
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    if(Input.GetMouseButtonDown(0))
        {
            TileStartDragPos = CurrentFramePos;
        }
        int start_x = Mathf.FloorToInt(TileStartDragPos.x);
        int end_x = Mathf.FloorToInt(CurrentFramePos.x);
        int start_y = Mathf.FloorToInt(TileStartDragPos.y);
        int end_y = Mathf.FloorToInt(CurrentFramePos.y);
        int start_z =  Mathf.FloorToInt(CurrentFramePos.z);
        int end_z = Mathf.FloorToInt(CurrentFramePos.z);
        if(end_x < start_x)
            {
                int tmp = end_x;
                end_x = start_x;
                start_x = tmp;
            }    
        if(end_y < start_y)
            {
                int tmp = end_y;
                end_y = start_y;
                start_y = tmp;
            }
            if(end_z < start_z)
            {
               int tmp = end_z;
                end_z = start_z;
                start_z = tmp; 
            }
            while(dragPreviewObjects.Count > 0)
            {
                GameObject GO = dragPreviewObjects[0];
                dragPreviewObjects.RemoveAt(0);
                SimplePool.Despawn(GO);
            }
    if(Input.GetMouseButton(0))
        {
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    for (int z = start_z; z < end_z; z++)
                    {
                    Tile t = World_Controller._Instance.World.GetTileAt(x,y,z);
                    if(t != null)
                    {
                       GameObject GO = SimplePool.Spawn(CursorPrefab,new Vector3(x,y,z),Quaternion.identity);
                       GO.transform.SetParent(this.transform,true);
                       dragPreviewObjects.Add(GO);
                    }
                    }
                }
            }
        }
        //End dragging
    if(Input.GetMouseButtonUp(0)&&(TileStartDragPos != null))
        {         
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = World_Controller._Instance.World.GetTileAt(x,y,0);
                    if(t != null)
                    {
                        t.Type = SelectedBuildTiles;
                    }
                }
            }
        }
    }
    void CameraMovement()
    {
    if(Input.GetMouseButton(2))
        {
            Vector3 diff = Last_Frame_Pos - NotOffsetCamera;
            Vector3 tmp = Camera.main.transform.position;
            Camera.main.transform.Translate(-diff.x,diff.y,0);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,tmp.z);
        }
       Last_Frame_Pos = GetWorldPositionOnPlane(Input.mousePosition,0);
       float z;       
                z = Camera.main.transform.position.z;
                z -=  z * Input.GetAxis("Mouse ScrollWheel");   
                z = Mathf.Clamp(z,20f,50f); 
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,z);   
    }
    int Click_Count = 0;
    void BuildingSelection()
    {
        Ray Mouse_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetMouseButtonDown(1))
        {
           if(Physics.Raycast(Mouse_Ray,out hit,100))
                {
                    BuildingHitbox("Factory",UI_Controller.LoadFactory,hit);
                    BuildingHitbox("Laboratory",UI_Controller.LoadLaboratory,hit);
                    BuildingHitbox("MainBase",UI_Controller.LoadMainBase,hit);
                }   
        }
    }
    private void BuildingHitbox(string ComponentName,Action LoadingFunction,RaycastHit RayHit)
    {
        if(RayHit.transform.gameObject.GetComponent(ComponentName) != null)
        {
            Click_Count +=1;
            if(Click_Count > 1)
                {
                    Click_Count = 0;
                    World_Controller._Instance.OnWorldMap = false;
                    LoadingFunction();
                    CurrentlySelectedBuilding = RayHit.transform.gameObject;
                        
                }
        }

    }
    private BuiltObject SelectedBuilding;
    private GameObject BuildingPreview ; // Preview of an object on the map that has yet to be placed 
    void Building()
    {
        Ray Mouse_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(SelectedBuilding != null)
        {
            Tile OriginTile = World_Controller._Instance.World.GetTileAt(1,1,1);
            BuildingPreview.layer = 2;
            if(Physics.Raycast(Mouse_Ray, out hit , 100))
            {

            //Showing where the building would be on the map
            BuildingPreview.transform.position = new Vector3((int)(hit.point.x),(int)(hit.point.y),(int)(hit.point.z));

            if(World_Controller._Instance.World.GetTileAt((int)(hit.point.x),(int)(hit.point.y),(int)(hit.point.z)) != null)
                {
                OriginTile = World_Controller._Instance.World.GetTileAt((int)(hit.point.x),(int)(hit.point.y),(int)(hit.point.z));
                }

            }
            if(Input.GetMouseButtonDown(0)&&World_Controller._Instance.Money >= SelectedBuilding.Money_Cost&&World_Controller._Instance.Substance >= SelectedBuilding.Substance_Cost)
            {
                //Placing the building Down
                BuildingPreview.layer = 0;
                GameObject Building = World_Controller._Instance.MakingBuilding(SelectedBuilding,OriginTile,BuildingPreview);
                if(Building != null)
                {

                Building.transform.position = BuildingPreview.transform.position;
                Building.transform.localScale = BuildingPreview.transform.localScale;

                World_Controller._Instance.AddResources(-SelectedBuilding.Money_Cost,-SelectedBuilding.Substance_Cost);
                UI_Controller._Instance.UpdateResources();
                Destroy(BuildingPreview.gameObject);
                BuildingPreview = new GameObject();

                if(SelectedBuilding.objectType == "MainBase") // Main Base is an exepction to the Algorithm because it can only be placed once at the start of the game
                {
                    World_Controller._Instance.PausedTime = false;
                    World_Controller._Instance.mainBase = Building.GetComponent<MainBase>();
                }
                SelectedBuilding = null;
                }
            }
            if(Input.GetMouseButtonDown(1)&&SelectedBuilding.objectType != "MainBase")
            {
                Destroy(BuildingPreview.gameObject);
                SelectedBuilding = null;
            }
        }
    }
    public void Laboratory()
    {
        SelectedBuilding = BuiltObject.CreatePrototype("Laboratory",3,4,3,150,40);
        BuildingPreview.name = "Laboratory";
        CreatingPreviewBuiling(BuildingPreview,LaboratoryModel);
        BuildingPreview.AddComponent<Laboratory>();
    }
    public void Factory()
    {
        SelectedBuilding = BuiltObject.CreatePrototype("Factory",3,2,2,50,5);
        BuildingPreview.name = "Factory";
        CreatingPreviewBuiling(BuildingPreview,FactoryModel);
        BuildingPreview.AddComponent<Factory>();
    }
    public void MainBase()
    {
        SelectedBuilding = BuiltObject.CreatePrototype("MainBase",3,3,3,0,0);
        BuildingPreview.name = "MainBase";
        CreatingPreviewBuiling(BuildingPreview,MainBaseModel);
        BuildingPreview.AddComponent<MainBase>();
        World_Controller._Instance.PausedTime = true;
    }
    // Creating Preview Building 
    // This method is responsible for changing a model of a Gameobject in a World Scene
    // Based on a Refrence model it will aplly Scale along with a collider 
    // After that if the player places down the buildiing it will inherit all components in the PreviewBuilding
    public void CreatingPreviewBuiling(GameObject PreviewBuilding,GameObject BuildingModel)
    {
        PreviewBuilding.AddComponent<MeshFilter>();
        PreviewBuilding.AddComponent<MeshRenderer>();
        PreviewBuilding.transform.localScale = BuildingModel.transform.localScale;
        PreviewBuilding.GetComponent<MeshFilter>().sharedMesh = BuildingModel.GetComponent<MeshFilter>().sharedMesh;
        PreviewBuilding.GetComponent<MeshRenderer>().sharedMaterials = BuildingModel.GetComponent<MeshRenderer>().sharedMaterials;
        PreviewBuilding.AddComponent<MeshCollider>();
    }
    public void SetMode_BuildRoad(TileType SelectedBuildTiles)
    {
        SelectedBuildTiles = TileType.Road;
    } 
}
