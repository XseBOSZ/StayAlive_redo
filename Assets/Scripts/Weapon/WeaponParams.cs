using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Scriptable/Weapon", fileName ="Weapon_")]

public class WeaponParams : ScriptableObject
{
    public Type weaponType;
    public Tier weaponTier;
    public float Damage;
    public float Durability;
    
}
[System.Serializable]
public enum Type 
{
    melee,
    pistol,
    rifle,
    sniper,
    SMG,
    granade,
    launcher,
    semiauto,
    auto
}
[System.Serializable]
public enum Tier
{
    basic,
    common,
    uncommon,
    rare,
    exotic,
    legendary,
    secret
}
