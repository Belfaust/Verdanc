using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Controller : MonoBehaviour
{
    public GameObject Cursor;
    Vector3 Last_Frame_Pos;
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
        Vector3 CurrentFramePos = GetWorldPositionOnPlane(Input.mousePosition,0);
        Cursor.transform.position = CurrentFramePos;

        Tile tileUnderMouse = GetTileAtWorldCoord(Cursor.transform.position- new Vector3(-.5f,-.5f));       
        if(tileUnderMouse!=null)
        {
        Cursor.SetActive(true);
        Vector3 cursorPosition = new Vector3(tileUnderMouse.X,tileUnderMouse.Y,0);
        Cursor.transform.position = cursorPosition;
        }else{Cursor.SetActive(false);
        }
        //Moving the Screen with the Middle Mouse Button
        if(Input.GetMouseButton(2))
        {
            Vector3 diff = Last_Frame_Pos - CurrentFramePos;
            Camera.main.transform.Translate(diff);
        }

       Last_Frame_Pos = GetWorldPositionOnPlane(Input.mousePosition,0);
       CentralizeCamera(-10);
    }
    void CentralizeCamera(float z)
    {
    if(Camera.main.transform.position.z != z)
       {
           Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,z);
       }
    }
    Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return World_Controller._Instance.World.GetTileAt(x , y);
    }
}
