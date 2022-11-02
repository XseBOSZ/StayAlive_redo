using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Ingridients
{
    public Item items;
    public int count;
};

[CreateAssetMenu(fileName ="Recipe_",menuName ="Scriptable/Recipe")]
public class RecipeCreator : ScriptableObject
{
    public string RecipeName;
    public Ingridients[] ingridients;
    public Ingridients[] results;
}
