using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : Interactable
{
    public Item item;
    public Image LoadingBar;

    public delegate void OnItemPickedUp(Item item);
    public static OnItemPickedUp OnItemPickedUpCallback; // an event which signals an item has been picked up
                                                         //anything that acts upon an item being picked up can be a listener 
                                                         //for example  the TextScroller class reacts by displaying certain text if the item is a powercell and it was the 1st one picked up
                                                         //the silo/rocket also react to this
    private KeyCode _interactKeyCode;
    private bool _loading;


    void Start()
    {
        if(LoadingBar != null)
        {
            LoadingBar.gameObject.SetActive(false);

            foreach (InputManager.Action action in GameManager.GM.InputManager.KeyBindings)
            {
                if (action.Name == "Interact")
                    _interactKeyCode = action.KeyCode;
            }
        }

        HealthManager.OnDamageTakenCallback += OnDamageTakenListener;
    }

    void OnDestroy()
    {
        HealthManager.OnDamageTakenCallback -= OnDamageTakenListener;
    }

    void Update()
    {
        if(_loading)
        {
            if (Input.GetKey(_interactKeyCode))
            {
                LoadingBar.fillAmount += Time.deltaTime / 2;
                if (LoadingBar.fillAmount >= 1)
                    PickUp();   
            }

            if (Input.GetKeyUp(_interactKeyCode))
            {
                _loading = false;
                LoadingBar.gameObject.SetActive(false);               
            }

            if(Input.anyKey)
            {
                foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(keycode) && keycode != _interactKeyCode)
                    {
                        _loading = false;
                        LoadingBar.gameObject.SetActive(false);
                    }                     
                }
            }
        }
    }

    void PickUp()
    {
        Debug.Log("PickUp()");

        _loading = false;

        if (GameManager.GM.Inventory.Add(item))
        {
            OnItemPickedUpCallback.Invoke(item);
            Destroy(gameObject);
        }
    }

    void OnDamageTakenListener()
    {
        if(_loading)
        {
            _loading = false;
            LoadingBar.gameObject.SetActive(false);
        }
    }

    public override void DoOnInteract()
    {
        
        if (LoadingBar == null)
            PickUp();
        else
        {
            _loading = true;
            LoadingBar.gameObject.SetActive(true);
            LoadingBar.fillAmount = 0;
        }
    }   
}
