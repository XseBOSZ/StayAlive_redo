using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIControler : MonoBehaviour
{
    public GameObject[] UI_list;
    public GameObject ConsoleContent;
    public GameObject PopUp;
    public GameObject ItemButton;
    public GameObject EQContent;
    public GameObject RecipeContent;

    public Backpack BP;
    public CraftingController CC;
    public ManipulatorItemUI MUI;
    private void Start()
    {
        for (int i = 0; i < UI_list.Length; i++)
        {
            UI_list[i].SetActive(false);
        }
        BP = GameObject.FindObjectOfType<Backpack>().GetComponent<Backpack>();
        CC = GameObject.FindObjectOfType<CraftingController>().GetComponent<CraftingController>();
        MUI = gameObject.GetComponent<ManipulatorItemUI>();
        DisablePopUp();
    }
    public void UpdateConsole(string message)
    {
        ConsoleContent.GetComponent<Text>().text = ConsoleContent.GetComponent<Text>().text + "\n" + message;
    }
    public void ClearConsole()
    {
        ConsoleContent.GetComponent<Text>().text = "";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isSthActive = false;
            for (int i = 0; i < UI_list.Length; i++)
            {
                if (UI_list[i].activeSelf)
                {
                    isSthActive = true;
                    break;
                }
            }
            if (isSthActive)
            {
                for (int i = 0; i < UI_list.Length; i++)
                {
                    UI_list[i].SetActive(false);
                }
            }
            else
            {
                Debug.Log("do pause menu");
            }
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            for (int i = 0; i < UI_list.Length; i++)
            {
                if (UI_list[i].name == "ConsoleUI")
                {
                    var isActive = UI_list[i].activeSelf;
                    UI_list[i].SetActive(!isActive);

                }
                else
                {
                    UI_list[i].SetActive(false);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < UI_list.Length; i++)
            {
                if (UI_list[i].name == "Equipment")
                {
                    var isActive = UI_list[i].activeSelf;
                    UI_list[i].SetActive(!isActive);
                    DrawEQ();
                }
                else
                {
                    UI_list[i].SetActive(false);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            for (int i = 0; i < UI_list.Length; i++)
            {
                if (UI_list[i].name == "Crafting")
                {
                    var isActive = UI_list[i].activeSelf;
                    UI_list[i].SetActive(!isActive);
                    DrawCraftingUI();
                }
                else
                {
                    UI_list[i].SetActive(false);
                }
            }
        }
    }
    public void EnablePopUp(string text)
    {
        PopUp.SetActive(true);
        PopUp.GetComponentInChildren<Text>().text = text;
    }
    public void DisablePopUp()
    {
        PopUp.SetActive(false);
        PopUp.GetComponentInChildren<Text>().text = "";
    }
    void DrawEQ()
    {
        for(int i = 0; i < EQContent.transform.childCount; i++)
        {
            Destroy(EQContent.transform.GetChild(i).gameObject);
        }
        for(int x = 0; x < BP.items.Count;x++)
        {
            var item = BP.items[x].item;
            var newButton = GameObject.Instantiate(ItemButton);
            newButton.transform.SetParent(EQContent.transform);
            newButton.transform.GetChild(0).GetComponent<Text>().text = BP.items[x].item.Name;
            newButton.transform.GetChild(1).GetComponent<Text>().text = BP.items[x].count.ToString();            
            newButton.GetComponent<Button>().onClick.AddListener(delegate { MUI.ShowItemUI(item); });
        }
    }
    void DrawCraftingUI()
    {
        for (int i = 0; i < RecipeContent.transform.childCount; i++)
        {
            Destroy(RecipeContent.transform.GetChild(i).gameObject);
        }
        Object[] recipes = Resources.LoadAll("Items/Recipes");
        foreach(Object obj in recipes)
        {
            var newButton = GameObject.Instantiate(ItemButton);
            newButton.transform.SetParent(RecipeContent.transform);
            RecipeCreator Recipe = Resources.Load("Items/Recipes/" + obj.name) as RecipeCreator;
            newButton.transform.GetChild(0).GetComponent<Text>().text = Recipe.results[0].items.Name;
            newButton.transform.GetChild(1).GetComponent<Text>().text = Recipe.results[0].count.ToString();
            newButton.GetComponent<Button>().onClick.AddListener(delegate { CC.ShowCraftingBox(Recipe); });
            
        }
    }

    
}

