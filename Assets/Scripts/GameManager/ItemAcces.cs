using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAcces : MonoBehaviour
{
    string resourcesPatch = "Items/Scriptable";
    UIControler UIC;
    private void Start()
    {
        UIC = gameObject.GetComponent<UIControler>();
        ValidateItems();
    }
    public void ValidateItems()
    {
        Item[] items = Resources.LoadAll<Item>(resourcesPatch);
        string prepraredMessage;
        for (int i = 0; i < items.Length; i++) {
            prepraredMessage = "object #"+items[i].ID + ": "+items[i].Name;
            UIC.UpdateConsole(prepraredMessage);
        }
    } 
}
