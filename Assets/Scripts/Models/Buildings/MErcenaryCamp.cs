using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MErcenaryCamp : MonoBehaviour
{
    int NumberOfMercenaries = 0;
    public int Height = 25;
    public int Width = 25;
    public Node[,] MapNodes;
    public Node[] MercenaryCamps = new Node[9];
    private void Awake() {
        MapNodes = new Node[Height,Width];
        for (int y = 0; y < Width; y++)
        {
            for (int x = 0; x < Height; x++)
            {
                MapNodes[y,x] = new Node(x,y);   
            }
        }
        for (int x = 0; x < MercenaryCamps.Length; x++)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {   
                    if(Random.Range(0,100)>95&&x<MercenaryCamps.Length)
                    {
                        MercenaryCamps[x] = MapNodes[i,j];  
                    }
                    else break;
                }
            }
        }
    }
}
public class Node
    {
        public int x{get;set;}
        public int y{get;set;}
        public int NumberofTroops{get;set;}
        public Node(int X,int Y)
        {
            this.x = X;
            this.y = Y;
        }
    }
