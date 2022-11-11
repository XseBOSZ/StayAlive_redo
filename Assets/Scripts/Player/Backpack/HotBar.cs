using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    public GameObject HotBarObject;

    [SerializeField]
    List<GameObject> Slots = new List<GameObject>();

    public List<BPItem> hotbarItems = new List<BPItem>();

    public Backpack BP;

    private void Start()
    {
        BP = GameObject.FindObjectOfType<Backpack>();
        for (int i = 0; i < HotBarObject.transform.childCount; i++) {

            Slots.Add(HotBarObject.transform.GetChild(i).gameObject);
        }
    }
    public void UseOnItem()
    {

    }
    public void AddToHotbar(BPItem itemToAdd)
    {

        for(int i = 0; i < HotBarObject.transform.childCount; i++)
        {
            var slotInfo = HotBarObject.transform.GetChild(i).GetComponent<HotbarSlot>();
            if (slotInfo.itemOnSlot.item != null)
            {
                if (slotInfo.itemOnSlot.item.Name == itemToAdd.item.Name)
                {
                    Debug.Log("added ammount to existing item ");
                    slotInfo.itemOnSlot.count += 1;
                    UpdateHotbar();
                    break;
                }
            }
            else if (slotInfo.itemOnSlot.item == null)
            {
                slotInfo.itemOnSlot = itemToAdd;
                hotbarItems.Add(itemToAdd);
                Debug.Log("added new item ");
                UpdateHotbar();
                break;
            }
            if (i == HotBarObject.transform.childCount && slotInfo.itemOnSlot.item != null)
            {
                Debug.Log("hotbar's full");
            }
        }
    }
    public void SwapItemOnBar(BPItem newItem, int slotToSwap)
    {
        hotbarItems[slotToSwap] = newItem;
        Slots[slotToSwap].GetComponent<HotbarSlot>().itemOnSlot = newItem;
        UpdateHotbar();
    }
    public void UpdateHotbar()
    {
        for(int i = 0; i < hotbarItems.Count; i++)
        {
            Slots[i].GetComponent<HotbarSlot>().UpdateSprite();
        }
    }
    public void RemoveFromHotbar(int slotToRemove)
    {
        BPItem emptyBPitem = new BPItem
        {
            item = null,
            count = 0
        };
        hotbarItems[slotToRemove] = emptyBPitem;
        Slots[slotToRemove].GetComponent<HotbarSlot>().itemOnSlot = emptyBPitem;
        UpdateHotbar();
    }
}
