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
    public void AddToHotbar(BPItem itemToAdd)
    {
        Debug.Log("engaged Add to hotbar");
        for(int i = 0; i < HotBarObject.transform.childCount; i++)
        {
            var slotInfo = HotBarObject.transform.GetChild(i).GetComponent<HotbarSlot>();

            if (slotInfo.itemOnSlot.item == null || slotInfo.itemOnSlot.count == 0)
            {
                slotInfo.itemOnSlot = itemToAdd;
                slotInfo.UpdateSprite();
                break;
            } else if (slotInfo.itemOnSlot.item == itemToAdd.item) {
                Debug.Log("item added to hotbar");

            }
            else if (i == HotBarObject.transform.childCount && slotInfo.itemOnSlot.item != null)
            {
                Debug.Log("hotbar's full");
            }
        }
    }
    public void RemoveFromHotbar()
    {

    }
}
