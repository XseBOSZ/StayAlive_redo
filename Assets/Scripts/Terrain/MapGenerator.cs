using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColorMap, Mesh};
    public DrawMode drawMode;


    public int octaves;
    public int seed;
    public const int mapChunkSize = 241;
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

    Queue<mapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<mapThreadInfo<MapData>>();
    Queue<mapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<mapThreadInfo<MeshData>>();
    public void requestMapData(Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(callback);
        };
        new Thread(threadStart).Start();
    }
    void MapDataThread(Action<MapData> callback)
    {
        MapData mapData = GenerateMapData();
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new mapThreadInfo<MapData>(callback, mapData));
        }
    }
    public void RequestMeshData(MapData mapData, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, callback);
        };
        new Thread(threadStart).Start();
    }
    void MeshDataThread(MapData mapData, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateMesh(mapData.heightMap, meshHeightMultiplier, meshHightCurve, levelOfDetail);
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
        MapData mapData = GenerateMapData();
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
            display.DrawMesh(MeshGenerator.GenerateMesh(mapData.heightMap, meshHeightMultiplier, meshHightCurve, levelOfDetail), TextureGeneration.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));

        }
    }

     MapData GenerateMapData()
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
        return new MapData(noiseMap, colorMap);
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