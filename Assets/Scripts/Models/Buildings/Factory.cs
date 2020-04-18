using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private Sapling[] MyFactory_Sapling_List = new Sapling[3];
    public Sapling[] Factory_Sapling_List
    {
    get {
            return MyFactory_Sapling_List;
        }
    set {
            MyFactory_Sapling_List= value;
            Incubator_Check();
        }
    } 
    void Incubator_Check()
    {
        for (int i = 0; i < Factory_Sapling_List.Length; i++)
        {            
            if(Factory_Sapling_List[i] != null && Factory_Sapling_List[i].Growing == false)
            {
                Factory_Sapling_List[i].Growing = true;
                StartCoroutine(Sapling_Growth(Factory_Sapling_List[i]));
                break;
            }
        }
    }
        IEnumerator Sapling_Growth(Sapling sapling)
    {
        Debug.Log("Growth_State = 1");
        yield return new WaitForSeconds((int)sapling.Growth_Time/2);
        Debug.Log("Growth_State = 2");
        sapling.Growth_State = 2;
        yield return new WaitForSeconds((int)sapling.Growth_Time/2);
        Debug.Log("Growth_State = 3");
        sapling.Growth_State = 3;

    }
}
