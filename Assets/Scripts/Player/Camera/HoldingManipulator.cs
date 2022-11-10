using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingManipulator : MonoBehaviour
{
    public GameObject HandHolder;
    public void PutInHand(string resourcesPath)
    {
        Debug.Log("loaded resource");
        /*var LoadedResource = Resources.Load(resourcesPath);
        Type T = LoadedResource.GetType();*/
        if (Resources.Load(resourcesPath).GetType() == typeof(GameObject))
        {
            GameObject.Instantiate(Resources.Load(resourcesPath), HandHolder.transform);
        }
        else
        {
            Debug.Log("your item cannot be held, do something else");
        }
    }
}
