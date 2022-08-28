using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/Soldier")]
public class SoldierCard : ScriptableObject
{
    public GameObject prefab;
    public int elixirAmount;
    public SoldierType soldierType;
}
