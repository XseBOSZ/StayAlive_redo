using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    public GameObject HotBarObject;
    [SerializeField]
    List<GameObject> Slots = new List<GameObject>();
    public List<BPItem> hotbarItems = new List<BPItem>();

    private void Start()
    {
        for (int i = 0; i < HotBarObject.transform.childCount; i++) {

            Slots.Add(HotBarObject.transform.GetChild(i).gameObject);
        }
    }
    public void UseOnItem()
    {

    }
}
