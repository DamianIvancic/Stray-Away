using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image Icon;
    public Text ItemCount;
    public Button RemoveButton;

    private Item item;

    public void UpdateItem(Item newItem, int itemCount)
    {
        if(item != newItem)
        item = newItem;

        if(Icon.sprite != item.Icon)
          Icon.sprite = item.Icon;

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
        InventoryManager.Instance.Inventory.Remove(item);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.EnableTooltip(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.DisableTooltip();
    }
}
