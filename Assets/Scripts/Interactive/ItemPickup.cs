using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override void DoOnInteract()
    {
        PickUp();      
    }

    void PickUp()
    {
        //add to inventory
        if(GameManager._GM.Inventory.Add(item))
             Destroy(gameObject);
    }

}
