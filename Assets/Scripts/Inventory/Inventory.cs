using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public int MaxSize = 20;

    private SortedDictionary<string, List<Item>> _items = new SortedDictionary<string, List<Item>>(); //easier to find items using a dictionary than a list

    public SortedDictionary<string, List<Item>> Items
    {
        get { return _items; }
    }

    public delegate void OnInventoryChange();
    public static OnInventoryChange OnInventoryChangeCallback;

    public bool Add(Item item) // called by ItemPickup.DoOnInteract()
    {
        if ((_items.ContainsKey(item.ItemName) && _items[item.ItemName].Count == 99) || (!_items.ContainsKey(item.ItemName) && _items.Count == MaxSize))
        {
            Debug.Log("Add()" + item.name + " false");
            return false;
        }

        if (!_items.ContainsKey(item.ItemName) && _items.Count < MaxSize)
            _items.Add(item.ItemName, new List<Item>());


        _items[item.ItemName].Add(item);

        OnInventoryChangeCallback.Invoke();

        Debug.Log("Add()" + item.name + " true");
        return true;
    }

    public void Remove(Item item) //called by the GUI buttons on the inventory slots so no null check needed
    {
        Debug.Log("InventoryManager.Remove(" + item.ItemName + ")");
        _items[item.ItemName].RemoveAt(_items[item.ItemName].Count - 1);

        if (_items[item.ItemName].Count == 0)
            _items.Remove(item.ItemName);

        OnInventoryChangeCallback.Invoke();
    }

    public int GetCount(Item item)
    {
        return _items[item.ItemName].Count;
    }

    public int GetCount(string itemName)
    {
        return _items[itemName].Count;
    }

    public bool Contains(string key)
    {
        return _items.ContainsKey(key);
    }
}
