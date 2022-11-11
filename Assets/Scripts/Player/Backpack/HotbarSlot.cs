using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public BPItem itemOnSlot;
    public Image slotImage;
    public string resourcesPath;
    public Sprite SpriteToChange;
    private void Start()
    {
        slotImage = gameObject.GetComponent<Image>();
        resourcesPath = "UI/Equipment/";
        UpdateSprite();
        
    }
    
    void UpdateSprite()
    {
        if (itemOnSlot.item == null || itemOnSlot.count == 0)
        {
            LoadResource("Slot_Blank");
            if (SpriteToChange != null) {

               slotImage.sprite = SpriteToChange;
                //loadedSprite = null;
            }
            else
            {

            }
        }
        else
        {

        }
        
    }

    void LoadResource(string resourceName)
    {
        var loadedResource = Resources.Load(resourcesPath+ resourceName) as Texture2D;
        Rect spriteRect = new Rect(0, 0, loadedResource.width, loadedResource.height);
        Sprite newSprite = Sprite.Create(loadedResource, spriteRect, new Vector2(0,0));
        SpriteToChange = newSprite;
    }

    //to not lose the concept - this script is being called by main hotbar script to simply do shit in ScriptableObject's and GameObject's
    //this will hold actual in game component and reference from backback to just click '1' on numpad and select #1 slot for example
    //to use/equip. This will become handy as long as i will keep the concept of hotbar 


}
