using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    Sapling[] Factory_Sapling_List = new Sapling[3];
        IEnumerator Sapling_Growth(Sapling sapling)
    {
        sapling.Growth_State = 1;
        yield return new WaitForSeconds((int)sapling.Growth_Time/2);
        sapling.Growth_State = 2;
        yield return new WaitForSeconds((int)sapling.Growth_Time/2);
        sapling.Growth_State = 3;

    }
}
