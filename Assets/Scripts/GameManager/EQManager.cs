using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EQManager : MonoBehaviour
{
    public Item lookingAt;
    public GameObject lookingAtObj;
    public Backpack BP;

    private void Start()
    {
        BP = GameObject.FindGameObjectWithTag("Player").GetComponent<Backpack>();
    }
    private void Update()
    {
        if (lookingAt != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpItem();
            }
        }
    }

    void PickUpItem()
    {
        if (BP.items.Count > 0)
        {
            for (int i = 0; i < BP.items.Count; i++)
            {
                if (BP.items[i].item.Name == lookingAt.Name)
                {
                    
                    int newAmmount = BP.items[i].count + 1;
                    BPItem ammount = new BPItem
                    {
                        item = BP.items[i].item,
                        count = newAmmount
                    };
                    BP.items[i] = ammount;
                    Destroyobj();
                    break;
                }
                else if (i == BP.items.Count-1)
                {
                    BPItem itemToadd = new BPItem();
                    itemToadd.item = lookingAt;
                    itemToadd.count = 1;
                    BP.items.Add(itemToadd);
                    Destroyobj();
                    break;
                } 
            }     
        }
        else
        {
            BPItem itemToadd = new BPItem();
            itemToadd.item = lookingAt;
            itemToadd.count = 1;
            BP.items.Add(itemToadd);
            Destroyobj();
        }
        
    }
    private void Destroyobj()
    {
       Destroy(lookingAtObj);
       lookingAt = null;
       lookingAtObj = null;
    }

}

