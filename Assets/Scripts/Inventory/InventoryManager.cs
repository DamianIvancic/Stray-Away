using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryParent;
    public InventorySlot SlotPrefab;
    public Transform SlotsPanel;

    public static InventoryManager Instance;

    private Inventory _inventory = new Inventory();
    private List<InventorySlot> _slots = new List<InventorySlot>();

    public Inventory Inventory
    {
        get { return _inventory;}
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Inventory.OnInventoryChangeCallback += UpdateUI;     
        }
        else
            Destroy(gameObject);
    }

    void UpdateUI()
    {
        int index = 0;

        foreach(string ItemName in _inventory.Items.Keys)
        {
            if(_slots.Count <= index)
            {
                InventorySlot clone = Instantiate(SlotPrefab, SlotsPanel);
                _slots.Add(clone);
            }

            _slots[index].UpdateItem(_inventory.Items[ItemName][0], _inventory.Items[ItemName].Count);
            index++;
        }

        if(_slots.Count > _inventory.Items.Count)
        {
            do
            {           
                _slots[index].ClearSlot();
                index++;

            } while (index < _slots.Count);
        }
    }

    public void Clear()
    {
        _inventory.Items.Clear();

        foreach (InventorySlot slot in _slots)
        {
            slot.ClearSlot();
        }
    }

    public void ToggleDisplay() //dont make the keycode Space or it will produce errors with GUI buttons that remove items from the inventory since space = submit under Project->Input
    {
        InventoryParent.gameObject.SetActive(!InventoryParent.gameObject.activeSelf);
    }
}
