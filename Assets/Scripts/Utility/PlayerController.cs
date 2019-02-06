using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerController : MonoBehaviour {

    public float Speed = 10;
    public int Damage = 10;

    public GameObject InteractSprite;
    public bool TextFinished = false;

    private Animator _anim;
    private Rigidbody2D _rb;
    private Interactable interactableScript;

    private float _movementH;
    private float _movementV;
    private Vector2 _movement;
    private bool _isSwinging;

    private AudioSource SwingSound;

    private static PlayerController Player;

    private void Start()
    {
        if(Player == null)
        {
            Player = this;
         
            _anim = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            SwingSound = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);       
        }       
    }

    // Update is called once per frame
    void Update () {

        if (GameManager._GM._gameState == GameManager.GameState.Playing)
        {
            UpdateMovement();               
            UpdateAnimator();  
        }
    }

    void UpdateMovement()
    {
        _movement = new Vector2(_movementH, _movementV);
        _movement.Normalize();

        switch ((int)(_movementH + _movementV))
        {
            case 0:
                _anim.SetBool("IsMoving", false);
                break;

            default:
                _anim.SetBool("IsMoving", true);
                break;
        }

        _rb.velocity = _movement * Speed;
    }

    void UpdateAnimator()
    {
        SetOrientation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Damageable")
        {
            collision.GetComponentInParent<EnemyScript>().TakeDamage(Damage);
        }

        if (collision.tag == "Interactable")
        {
            InteractSprite.SetActive(true);
            interactableScript = collision.GetComponent<Interactable>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
            Interact();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            InteractSprite.SetActive(false);
            interactableScript = null;
        }
    }

   
    private void SetOrientation()
    {
        switch ((int)_movementH)
        {
            case -1:
                _anim.SetBool("Right", false);
                _anim.SetBool("Left", true);
                break;

            case 1:
                _anim.SetBool("Right", true);
                _anim.SetBool("Left", false);
                break;

            case 0:
                _anim.SetBool("Right", false);
                _anim.SetBool("Left", false);
                break;

            default:
                break;
        }

        switch ((int)_movementV)
        {
            case -1:
                _anim.SetBool("Up", false);
                _anim.SetBool("Down", true);
                break;

            case 1:
                _anim.SetBool("Up", true);
                _anim.SetBool("Down", false);
                break;

            case 0:
                _anim.SetBool("Up", false);
                _anim.SetBool("Down", false);
                break;

            default:
                break;
        }
    }

    #region Callbacks - > All the callback functions go here

    public void MoveLeft()
    {
        _movementH = -1;
    }

    public void MoveRight()
    {
        _movementH = 1;
    }

    public void MoveDown()
    {
        _movementV = -1;
    }

    public void MoveUp()
    {
        _movementV = 1;
    }

    public void StopMovingVertical()
    {
        _movementV = 0;
    }

    public void StopMovingHorizontal()
    {
        _movementH = 0;
    }

    public void Interact()
    {
        if (InteractSprite.activeSelf)
        {
            interactableScript.DoOnInteract();
            InteractSprite.SetActive(false);
        }
    }

    public void Swing()
    {
        SwingSound.Play();
        _anim.SetTrigger("IsSwinging");
    }

    public void Inventory() //dont make the keycode Space or it will produce errors with GUI buttons that remove items from the inventory since space = submit under Project->Input
    {
        GameObject Inventory = GameManager._GM.Inventory.gameObject;
        Inventory.SetActive(!Inventory.activeSelf);

        Debug.Log("PlayerController.Inventory()");
    }

    public void RegisterCallbacks()
    {
        List<InputManager.Action> KeyBindings = GameManager._GM.InputManager.KeyBindings;

        foreach (InputManager.Action action in KeyBindings)
        {
            switch (action.Name)
            {
                case ("Up"):
                    action.ActionCallBack += MoveUp;
                    action.StopActionCallBack += StopMovingVertical;
                    break;
                case ("Left"):
                    action.ActionCallBack += MoveLeft;
                    action.StopActionCallBack += StopMovingHorizontal;
                    break;
                case ("Down"):
                    action.ActionCallBack += MoveDown;
                    action.StopActionCallBack += StopMovingVertical;
                    break;
                case ("Right"):
                    action.ActionCallBack += MoveRight;
                    action.StopActionCallBack += StopMovingHorizontal;
                    break;
                case ("Attack"):
                    action.ActionCallBack += Swing;
                    break;
                case ("Interact"):
                    action.ActionCallBack += Interact;
                    break;
                case ("Inventory"):
                    action.ActionCallBack += Inventory;
                    break;
            }
        }     
    }

    #endregion 
}
