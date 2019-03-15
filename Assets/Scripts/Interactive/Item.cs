using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite Icon = null;
    public string ItemName;
    public int ItemValue;
    public string ItemDescription;
    
}
