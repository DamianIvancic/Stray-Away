using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image Icon;
    public Text ItemCount;
    public Button RemoveButton;

    private Item item;

    public void UpdateItem(Item newItem, int itemCount)
    {
        if(item != newItem)
        item = newItem;

        if(Icon.sprite != item.icon)
          Icon.sprite = item.icon;

        Icon.enabled = true;
        ItemCount.text = "x " + itemCount;
        RemoveButton.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        item = null;
        Icon.sprite = null;
        ItemCount.text = null;
        RemoveButton.gameObject.SetActive(false);
    }

    public void OnRemoveButton()
    {
        Debug.Log("OnRemoveButton()");
        GameManager.GM.Inventory.Remove(item);
        //RemoveButton.interactable = false;
    }
}
