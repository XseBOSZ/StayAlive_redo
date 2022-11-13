using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class TerrainData : UpdatableData
{
    public float uniformScale = 5f;
    public AnimationCurve meshHightCurve;
    public float meshHeightMultiplier;
    public bool useFalloff;
    public bool useFlatShadeing;


    public float minHeight
    {
        get
        {
            return uniformScale * meshHeightMultiplier * meshHightCurve.Evaluate(0);
        }
    }
    public float maxHeight
    {
        get
        {
            return uniformScale * meshHeightMultiplier * meshHightCurve.Evaluate(1);
        }
    }

}
