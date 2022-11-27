using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour
{
    public GameObject IngidientsHolderBox;
    public GameObject ResultsHolderBox;
    public GameObject ItemBox;
    public GameObject CraftingBox;
    public GameObject CraftBttn;
    //public GameObject OBJTORESET;

    public Backpack BP;
    HotBar bar;

    private void Start()
    {
        BP = GameObject.FindObjectOfType<Backpack>().GetComponent<Backpack>();
        bar = GameObject.FindObjectOfType<HotBar>();
    }
    public void ShowCraftingBox(RecipeCreator recipe)
    {
        

        ClearBoxex();
        for (int i = 0; i < recipe.ingridients.Length; i++)
        {
            var box = GameObject.Instantiate(ItemBox);
            box.transform.SetParent(IngidientsHolderBox.transform);
            string name = recipe.ingridients[i].items.Name;
            int ammount = recipe.ingridients[i].count;
            box.transform.GetChild(0).GetComponent<Text>().text = name;
            box.transform.GetChild(1).GetComponent<Text>().text = ammount.ToString();
            StartCoroutine(UpdateLayoutGroup());
        }
        for (int i = 0; i < recipe.results.Length; i++)
        {
            var box = GameObject.Instantiate(ItemBox);
            box.transform.SetParent(ResultsHolderBox.transform);
            string name = recipe.results[i].items.Name;
            int ammount = recipe.results[i].count;
            box.transform.GetChild(0).GetComponent<Text>().text = name;
            box.transform.GetChild(1).GetComponent<Text>().text = ammount.ToString();
            CraftBttn.GetComponent<Button>().onClick.AddListener(delegate { Check(recipe); });
            StartCoroutine(UpdateLayoutGroup());
        }

        StartCoroutine(UpdateLayoutGroup());

    }
    void ClearBoxex()
    {
        for (int x = 0; x < IngidientsHolderBox.transform.childCount; x++)
        {
            if (IngidientsHolderBox.transform.childCount > 0)
                Destroy(IngidientsHolderBox.transform.GetChild(x).gameObject);
        }
        for (int x = 0; x < ResultsHolderBox.transform.childCount; x++)
        {
            if (ResultsHolderBox.transform.childCount > 0)
                Destroy(ResultsHolderBox.transform.GetChild(x).gameObject);
        }
    }
    IEnumerator UpdateLayoutGroup()
    {
        CraftingBox.SetActive(false);
        yield return new WaitForEndOfFrame();
        CraftingBox.SetActive(true);
    }
    public void Check(RecipeCreator recipe)
    {
        bool canCraft = false;
        for (int i = 0; i < recipe.ingridients.Length; i++)
        {
            for (int x = 0; x < BP.items.Count; x++)
            {
                if (BP.items[x].item == recipe.ingridients[i].items)
                {
                    if (BP.items[x].count >= recipe.ingridients[i].count)
                    {
                        Debug.Log("i have: " + BP.items[x].count + " of " + recipe.ingridients[i].count + " needed item {" + BP.items[x].item.Name + "}");
                        canCraft = true;
                    }
                    else
                    {
                        Debug.Log("not enough items");
                        canCraft = false;
                    }
                }
            }
        }
        if (canCraft)
        {
            Craft(recipe);
        }

    }
    void Craft(RecipeCreator recipe)
    {
        for (int i = 0; i < recipe.ingridients.Length; i++)
        {
            BPItem item = new BPItem();
            item.item = recipe.ingridients[i].items;
            item.count = recipe.ingridients[i].count;

            for (int x = 0; x < BP.items.Count; x++)
            {
                if(BP.items[x].item == item.item)
                {
                    if (BP.items[x].count > item.count)
                    {
                        BPItem newitem = new BPItem();
                        newitem.item = recipe.ingridients[i].items;
                        newitem.count = recipe.ingridients[i].count;
                        BP.items[x] = newitem;
                    }
                    else
                    {
                        BP.items.Remove(item);
                    }
                }
                
            }
        }

        for(int i = 0; i < recipe.results.Length; i++)
        {
            BPItem newItem = new BPItem();
            newItem.item = recipe.results[i].items;
            newItem.count = recipe.results[i].count;
            bar.AddToHotbar(newItem);
            BP.items.Add(newItem);
        }
    }
}

