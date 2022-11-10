using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Axe : MonoBehaviour, AFunctions
{
    Camera mainCamera;
    HoldingManipulator hm;


    private void Start()
    {
        
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

    public void Use(Item RefItem)
    {
        if (hm == null)
        {
            hm = GameObject.FindObjectOfType<HoldingManipulator>();
            if (hm != null)
            {
                hm.GetComponent<HoldingManipulator>().PutInHand(RefItem.ResourcesPath);
            }
            else
            {
                Debug.Log("unable to locate holding manipulator");
            }
        }
        else
        {
            hm.GetComponent<HoldingManipulator>().PutInHand(RefItem.ResourcesPath);
        }
    }
}
