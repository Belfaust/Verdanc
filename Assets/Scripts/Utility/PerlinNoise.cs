using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise 
{
    long seed;
    public PerlinNoise(long seed) => this.seed = seed;
    private int random(int x,int range)
    {
        return (int)((x*seed)^5)%range;
    }
    public int getNoise(int x,int range)
    {
        int chunkSize = 25;
        int chunkIndex = x/chunkSize;
        float progress = (x % chunkSize) / (chunkSize* 1f);

        float l_random = random(chunkIndex,range);
        float r_random = random(chunkIndex+1,range);

        float noise = (1-progress) * l_random + progress + r_random;

        return (int)Mathf.Round(noise);
    }
}
