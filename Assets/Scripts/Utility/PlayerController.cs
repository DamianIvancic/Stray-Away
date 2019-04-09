using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour {

    public float Speed = 15;
    public int Damage = 1;

    public GameObject InteractSprite;
   
    [HideInInspector]
    public Interactable interactableScript;
    [HideInInspector]
    public AudioSource swingSound;
    private Animator _anim;
    [HideInInspector]
    public Rigidbody2D RB;

    private float _movementH;
    private float _movementV;
    private Vector2 _movement;
    private bool _isSwinging;
    private Vector2 _startingPos;

    private void Start()
    {
        if (GameManager.GM.Player == null)
        {
            GameManager.GM.Player = this;

            _anim = GetComponentInChildren<Animator>();
            RB = GetComponent<Rigidbody2D>();
            swingSound = GetComponent<AudioSource>();

            _startingPos = new Vector2(87, 19);

            RegisterCallbacks();

            SceneManager.sceneLoaded += OnSceneLoadedListener;

            DontDestroyOnLoad(gameObject);
        }
        else               
            Destroy(gameObject);
        
    }

    // Update is called once per frame
    void Update () {

        if (GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            UpdateMovement();
            UpdateAnimator();
        }
        else if(GameManager.GM.gameState == GameManager.GameState.Paused || GameManager.GM.gameState == GameManager.GameState.GameOver)
        {
            RB.velocity = Vector2.zero;
            _anim.SetBool("IsMoving", false);
        }
    }

    void UpdateMovement()
    {
        _movement = new Vector2(_movementH, _movementV);
        _movement.Normalize();

        RB.velocity = _movement * Speed;

        if (RB.velocity.magnitude == 0f)
            RB.isKinematic = true; //prevents enemies from pushing the player
        else
            RB.isKinematic = false;
    }

    void UpdateAnimator()
    {
        if(RB.velocity.magnitude > 0)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        SetOrientation();
    }

    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode) //listener for SceneManager.sceneLoaded
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (SceneIndex)
        {         
            case(1):
                gameObject.SetActive(true);
                transform.position = _startingPos;
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }


    private void SetOrientation()
    {
        
        if(RB.velocity.x < 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", true);
        }
        else if(RB.velocity.x == 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", false);
        }
        else if(RB.velocity.x > 0)
        {
            _anim.SetBool("Right", true);
            _anim.SetBool("Left", false);
        }


        if(RB.velocity.y <0)
        {
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", true);
        }
        else if(RB.velocity.y == 0)
        {
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", false);
        }
        else if(RB.velocity.y > 0)
        {
            _anim.SetBool("Up", true);
            _anim.SetBool("Down", false);
        }
    }


    #region Collisions -> all the collision/trigger functions go here
    //interacting with objects is done via the interact callback which can be validly called whenever the InteractSprite is active 
    //the sprite gets activated from the interactable script's trigger/collision methods (so they can be overriden if needed)

    void OnTriggerEnter2D(Collider2D collision)
    {
          if (collision.tag == "Damageable")     
              collision.GetComponentInParent<Enemy>().TakeDamage(Damage);       
    }

    #endregion -> all the  -> all the collision/trigger functions go here


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


    public void ResetMovement()
    {
        _movement = Vector2.zero;
        _movementH = 0;
        _movementV = 0;
        RB.velocity = _movement;

        UpdateAnimator();
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
        _anim.SetTrigger("IsSwinging");
    }

    public void Inventory() //dont make the keycode Space or it will produce errors with GUI buttons that remove items from the inventory since space = submit under Project->Input
    {
        GameObject Inventory = GameManager.GM.Inventory.gameObject;
        Inventory.SetActive(!Inventory.activeSelf);

        Debug.Log("PlayerController.Inventory()");
    }

    public void RegisterCallbacks()
    {
        List<InputManager.Action> KeyBindings = GameManager.GM.InputManager.KeyBindings;

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
                /*case ("Inventory"):
                    action.ActionCallBack += Inventory;
                    break;*/
            }
        }     
    }

    #endregion 
}
