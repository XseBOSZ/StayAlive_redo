using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Axe : MonoBehaviour, AFunctions
{
    public Item CorespondingSO;
    HoldingManipulator hm;
    private void Start()
    {
        hm = GameObject.FindObjectOfType<HoldingManipulator>();
    }
    public void Combine()
    {
        Debug.Log("Combine");
    }

    public void Drop()
    {
        Debug.Log("Drop");
    }

    public void Inspect()
    {
        Debug.Log("Inspect");
    }

    public void Use()
    {
        hm.GetComponent<HoldingManipulator>().PutInHand(CorespondingSO.ResourcesPath);
    }
}
