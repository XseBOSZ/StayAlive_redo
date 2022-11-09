using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject,AFunctions
{
    public int ID;
    public string Name;
    public string Description;
    public string ResourcesPath;
    public WeaponParams WeaponReference;
    public GameObject ObjectReference;
    public ItemType Itemtype;

    public void Combine()
    {
        ObjectReference.GetComponent<AFunctions>().Combine();
    }

    public void Drop()
    {
        ObjectReference.GetComponent<AFunctions>().Drop();
    }

    public void Inspect()
    {
        ObjectReference.GetComponent<AFunctions>().Inspect();
    }

    public void Use()
    {
        ObjectReference.GetComponent<AFunctions>().Use();
    }

    //public GameObject Prefab;
    //public GameObject PrefabinHand;
    //public Texture2D thumbnail;

    private void Awake()
    {
        if (Itemtype != ItemType.Weapon)
        {
            WeaponReference = null;
        }
    }
}
[System.Serializable]
public enum ItemType
{
    Weapon,
    Food,
    Drink,
    Component,
    Enviroment
}
