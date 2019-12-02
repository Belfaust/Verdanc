using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise 
{
    long seed;
    public PerlinNoise(long seed) => this.seed = seed;
    private int random(long x,int range)
    {
        return (int)((x*seed)^5)%range;
    }
    private int random(long x,long y,int range)
    {
        //return (int)((x*seed)^5)%range;
        return random(x +y *234513, range);
    }
    public int getNoise(int x,int z,int range)
    {
        int chunkSize = 5;
         
         float noise = 0;

         range /= 2;
         while(chunkSize > 0)
         {
        int index_x = x/chunkSize;
        int index_y = z/chunkSize;

        float t_x = (x % chunkSize) / (chunkSize* 1f);
        float t_y = (x % chunkSize) / (chunkSize* 1f);

        float r_00 = random(index_x,  index_y,range);
        float r_01 = random(index_x,  index_y+1,range);
        float r_10 = random(index_y+1,index_y,range);
        float r_11 = random(index_y+1,index_y+1,range);

        float r_0 = lerp(r_00,r_01,t_y);
        float r_1 = lerp(r_10,r_11,t_y);

        float r = lerp(r_0,r_1,t_x);

        noise += r;

        chunkSize /=2;
        range /= 2;

        range = Mathf.Max(1,range);
         }
    return (int)Mathf.Round(noise);
    }
    private float lerp(float l,float r,float t)
    {
        return l * (1-t) + r * t;
    }
}
