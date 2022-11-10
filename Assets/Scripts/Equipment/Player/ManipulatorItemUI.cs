using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManipulatorItemUI : MonoBehaviour
{
    public GameObject ItemUI;
    public GameObject buttonHolder;
    public GameObject button;
    public GameObject WeaponHolder;
    public Button DropButton;
    public Button InspectButton;
    public Button CombineButton;
    public Button DeleteButton;

    public Backpack BP;
    
    private void Start()
    {
        BP = GameObject.FindObjectOfType<Backpack>().GetComponent<Backpack>();
        ItemUI.SetActive(false);
    }

    public void ShowItemUI(Item slotNumber)
    {
        DrawButtons(slotNumber);
    }
    void DrawButtons(Item SlotInBP)
    {
        ClearChildrens(buttonHolder);
        Debug.Log("holder is enabled");
        if (SlotInBP.Itemtype == ItemType.Weapon)
        {
            var newButton = GameObject.Instantiate(button);
            newButton.transform.SetParent(buttonHolder.transform);
            newButton.GetComponentInChildren<Text>().text = "Equip";
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SlotInBP.Use(SlotInBP); });
        }
        if (SlotInBP.Itemtype == ItemType.Food)
        {
            var newButton = GameObject.Instantiate(button);
            newButton.transform.SetParent(buttonHolder.transform);
            newButton.GetComponentInChildren<Text>().text = "Eat";
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SlotInBP.Use(SlotInBP); });

        }
        if (SlotInBP.Itemtype == ItemType.Drink)
        {
            var newButton = GameObject.Instantiate(button);
            newButton.transform.SetParent(buttonHolder.transform);
            newButton.GetComponentInChildren<Text>().text = "Drink";
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SlotInBP.Use(SlotInBP); });

        }
        if (SlotInBP.Itemtype == ItemType.Component)
        {
            var newButton = GameObject.Instantiate(button);
            newButton.transform.SetParent(buttonHolder.transform);
            newButton.GetComponentInChildren<Text>().text = "Use";
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SlotInBP.Use(SlotInBP); });

        }

        DropButton.onClick.AddListener(delegate { SlotInBP.Drop(); });
        InspectButton.onClick.AddListener(delegate { SlotInBP.Inspect(); });
        CombineButton.onClick.AddListener(delegate { SlotInBP.Combine(); });
        //DeleteButton.onClick.AddListener(delegate { SlotInBP.De(); });

        ItemUI.SetActive(true);

    }
    //todo: Delete button
    void DebugItem(Item debugingItem)
    {
        Debug.Log("item: " + debugingItem.Name + " is type of: "+debugingItem.Itemtype);
    }
    void ClearChildrens(GameObject parent)
    {
        ItemUI.SetActive(false);
        Debug.Log("holder is disabled");
        if (parent.transform.childCount > 0)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).tag != "DontClear")
                {
                    Destroy(parent.transform.GetChild(i).gameObject);
                    Debug.Log("holder is cliring..");
                }
            }
        }
        else {
            Debug.Log("holder is empty");
        }
    }
}
