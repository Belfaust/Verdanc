using System.Collections;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public enum DrawMode{NoiseMap,ColorMap};
    public DrawMode drawMode;

public int Width;
public int Height;
public float noiseScale;

public int octaves;
[Range(0,1)]
public float persistance;
public float lacunarity;

public int seed;
public Vector2 offset;

public TerrainType[] regions;
public bool autoUpdate;
public void GenerateMap()
{
float[,] noiseMap = Noise.GenerateNoiseMap(Width,Height,seed,noiseScale,octaves,persistance,lacunarity,offset);

Color[] colorMap = new Color[Width * Height];
for (int y = 0; y < Height; y++)
{
    for (int x = 0; x < Width; x++)
    {
        float currentHeight = noiseMap[x,y];
        for (int i = 0; i < regions.Length; i++)
        {
            if(currentHeight <= regions[i].height)
            {
                colorMap[y*Width+x] = regions[i].color;
                break;
            }
        }
    }
}

MapDisplay diplay = FindObjectOfType<MapDisplay>();
if(drawMode== DrawMode.NoiseMap)
{
diplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
}   else if(drawMode == DrawMode.ColorMap)
{
diplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap,Width,Height));
}
}

private void OnValidate() {
            if(Height<1)
            {
                Height = 1;
            }
            if(Width < 1)
            {
                Width = 1;
            }
            if(lacunarity < 1 )
            {
                lacunarity = 1;
            }
            if(octaves < 0)
            {
                octaves = 0;
            }
            
}
[System.Serializable]
public struct TerrainType{
    public string name;
    public float height;
    public Color color;


}
}
