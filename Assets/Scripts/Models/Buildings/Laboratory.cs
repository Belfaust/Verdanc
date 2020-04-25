using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laboratory : MonoBehaviour
{
    private Sapling[] MyLaboratory_Sapling_List = new Sapling[3];
    public float[] ResearchSlotProgress = new float[3];
    public Sapling[] Laboratory_Sapling_List
    {
    get {
            return MyLaboratory_Sapling_List;
        }
    set {
            MyLaboratory_Sapling_List= value;
            Sapling_Research_Check();
        }
    } 
    void Sapling_Research_Check()
    {
        for (int i = 0; i < Laboratory_Sapling_List.Length; i++)
        {
            if(Laboratory_Sapling_List[i] != null&&Laboratory_Sapling_List[i].UnderResearch == false)
            {
                StartCoroutine(Sapling_Research(Laboratory_Sapling_List[i]));
                Laboratory_Sapling_List[i].UnderResearch = true;
            }
        }
    }
    IEnumerator Sapling_Research(Sapling sapling)
    {
        int Timer = 0;
        Debug.Log("Begininng Research of " + sapling);
        for (int i = 0; i < sapling.Fertility + ( sapling.Verdancy * 3 ) + sapling.Growth_Time; i++)
        {
            yield return new WaitForSeconds(1);
            for (int j = 0; j < Laboratory_Sapling_List.Length; j++)
            {
                if(Laboratory_Sapling_List[j] == sapling)
                {
                    ResearchSlotProgress[j] = i/sapling.Fertility + ( sapling.Verdancy * 3 ) + sapling.Growth_Time;
                }
            }
            Timer += 1;
        }
        sapling.Fertility = sapling.Fertility + Random.Range((int)0, 3);

        //Here should be a code for Returning Sapling to inventory

        for (int i = 0; i < Laboratory_Sapling_List.Length; i++)
        {
            if(Laboratory_Sapling_List[i] == sapling)
            {
                Laboratory_Sapling_List[i].UnderResearch = false;
                Laboratory_Sapling_List[i] = null;
            }
        }

        Debug.Log("Research Complete");
    }
}
