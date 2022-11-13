using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColorMap, Mesh, FalloffMap};
    public Noise.NormalizeMode normalizedMode;
    
    public DrawMode drawMode;


    public int octaves;
    public int seed;
    public const int mapChunkSize = 95;
    [Range(0,6)]
    public int EditorLOD;

    public AnimationCurve meshHightCurve;
    
    public float noiseScale;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public float meshHeightMultiplier;
    float[,] falloffMap;

    public Vector2 offset;

    public bool autoUpdate;
    public bool useFalloff;
    public bool useFlatShadeing;

    public TerrainType[] regions;

    Queue<mapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<mapThreadInfo<MapData>>();
    Queue<mapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<mapThreadInfo<MeshData>>();
    private void Awake()
    {
        falloffMap = Falloff.GenerateFalloff(mapChunkSize);
    }
    public void requestMapData(Vector2 Center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(Center, callback);
        };
        new Thread(threadStart).Start();
    }
    void MapDataThread(Vector2 Center, Action<MapData> callback)
    {
        MapData mapData = GenerateMapData(Center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new mapThreadInfo<MapData>(callback, mapData));
        }
    }
    public void RequestMeshData( MapData mapData,int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData,lod, callback);
        };
        new Thread(threadStart).Start();
    }
    void MeshDataThread(MapData mapData,int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateMesh(mapData.heightMap, meshHeightMultiplier, meshHightCurve, lod,useFlatShadeing);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new mapThreadInfo<MeshData>(callback, meshData));
        }
    }
    private void Update()
    {
        if(mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                mapThreadInfo<MapData> thredinfo = mapDataThreadInfoQueue.Dequeue();
                thredinfo.callback(thredinfo.parameter);
            }
        }
        if(meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                mapThreadInfo<MeshData> thredInfo = meshDataThreadInfoQueue.Dequeue();
                thredInfo.callback(thredInfo.parameter);
            }
        }
    }

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTextureMethod(TextureGeneration.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTextureMethod(TextureGeneration.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateMesh(mapData.heightMap, meshHeightMultiplier, meshHightCurve, EditorLOD,useFlatShadeing), TextureGeneration.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));

        }else if(drawMode == DrawMode.FalloffMap)
        {
            display.DrawTextureMethod(TextureGeneration.TextureFromHeightMap(Falloff.GenerateFalloff(mapChunkSize)));
        }
    }

     MapData GenerateMapData(Vector2 center)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(normalizedMode,mapChunkSize+2, mapChunkSize+2, seed ,noiseScale,octaves,persistance,lacunarity, center + offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y] - falloffMap[x, y],0,1);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight >= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;  
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colorMap);
    }
    private void OnValidate()
    {
        falloffMap = Falloff.GenerateFalloff(mapChunkSize);
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }

    struct mapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public mapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
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

public struct MapData{
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}