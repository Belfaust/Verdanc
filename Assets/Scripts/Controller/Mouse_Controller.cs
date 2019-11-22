using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mouse_Controller : MonoBehaviour
{
    public GameObject CursorPrefab;
    Tile.TileType SelectedBuildTiles = Tile.TileType.Road;
    Vector3 CurrentFramePos = new Vector3(-.5f,-.5f);//ASk on next lessons
    Vector3 Last_Frame_Pos;
    Vector3 NotOffsetCamera;
    Vector3 TileStartDragPos;
    List<GameObject> dragPreviewObjects = new List<GameObject>();
    public float offset_z = 2;
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
     Ray ray = Camera.main.ScreenPointToRay(screenPosition);
     Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
     float distance;
     xy.Raycast(ray, out distance);    
     return ray.GetPoint(distance);
 }    
     void Update() 
    {
        CurrentFramePos = new Vector3(GetWorldPositionOnPlane(Input.mousePosition,0).x +.5f,GetWorldPositionOnPlane(Input.mousePosition,0).y +.5f);
        NotOffsetCamera = GetWorldPositionOnPlane(Input.mousePosition,0);
        //UpdateCursor();
        UpdateDragging();
        CameraMovement();       
        Scroling();
    }
    void Scroling()
    {
        float z;       
                z = Camera.main.transform.position.z;
                z -=  z * Input.GetAxis("Mouse ScrollWheel");   
                z = Mathf.Clamp(z,-35f,-5f); 
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,z);   
               
    }
    // void UpdateCursor()
    // {        
    //     Tile tileUnderMouse = World_Controller._Instance.GetTileAtWorldCoord(Cursor.transform.position);       
    //     if(tileUnderMouse!=null)
    //     {
    //     Cursor.SetActive(true);
    //     Vector3 cursorPosition = new Vector3(tileUnderMouse.X,tileUnderMouse.Y,0);
    //     Cursor.transform.position = cursorPosition;
    //     }else{Cursor.SetActive(false);
    //     }
    // }
    void UpdateDragging()
    {
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
                    Tile t = World_Controller._Instance.World.GetTileAt(x,y);
                    if(t != null)
                    {
                       GameObject GO = SimplePool.Spawn(CursorPrefab,new Vector3(x,y,-1),Quaternion.identity);
                       GO.transform.SetParent(this.transform,true);
                       dragPreviewObjects.Add(GO);
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
                    Tile t = World_Controller._Instance.World.GetTileAt(x,y);
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
            Camera.main.transform.Translate(diff.x,diff.y,0);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,tmp.z);
        }
       Last_Frame_Pos = GetWorldPositionOnPlane(Input.mousePosition,0);
    }

    public void SetMode_BuildRoad()
    {
        SelectedBuildTiles = Tile.TileType.Road;
    } 
    public void SetMode_BuildGrass()
    {
        SelectedBuildTiles = Tile.TileType.Grass;
    }
    public void SetMode_BuildWater()
    {
        SelectedBuildTiles = Tile.TileType.Water;
    }
}
