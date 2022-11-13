using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class NoiseData : UpdatableData
{

    //floats
    public float noiseScale;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    //ints
    public int octaves;
    public int seed;

    //vectors
    public Vector2 offset;


    //enums
    public Noise.NormalizeMode normalizedMode;


    protected override void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        base.OnValidate();
    }
}
