using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;


    public int octaves;
    public int seed;
    const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;

    public AnimationCurve meshHightCurve;
    
    public float noiseScale;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public float meshHeightMultiplier;

    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed ,noiseScale,octaves,persistance,lacunarity, offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTextureMethod(TextureGeneration.TextureFromHeightMap(noiseMap));
        }else if(drawMode == DrawMode.ColorMap)
        {
            display.DrawTextureMethod(TextureGeneration.TextureFromColorMap(colorMap,mapChunkSize,mapChunkSize));
        }else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateMesh(noiseMap, meshHeightMultiplier, meshHightCurve,levelOfDetail), TextureGeneration.TextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
            
        }
    }
    private void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}