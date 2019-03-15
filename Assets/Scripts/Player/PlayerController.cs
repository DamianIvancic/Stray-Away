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

    private Animator _anim;
    [HideInInspector]
    public Rigidbody2D _rb;
    private AudioSource _swingSound;

    private float _movementH;
    private float _movementV;
    private Vector2 _movement;
    private bool _isSwinging;

    void Start()
    {     
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _swingSound = GetComponent<AudioSource>();

        InputManager.Instance.RegisterCallbacks();    
    }

    // Update is called once per frame
    void Update () {

        if (GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            UpdateMovement();
            UpdateAnimator();

            if (_rb.isKinematic == true)
            {
                _rb.isKinematic = false;
            }

        }
        else if(GameManager.GM.gameState == GameManager.GameState.Paused || GameManager.GM.gameState == GameManager.GameState.GameOver)
        {
            _rb.velocity = Vector2.zero;
            _anim.SetBool("IsMoving", false);
        }
    }

    void UpdateMovement()
    {
        _movement = new Vector2(_movementH, _movementV);
        _movement.Normalize();

        _rb.velocity = _movement * Speed;
    }

    void UpdateAnimator()
    {
        if(_rb.velocity.magnitude > 0)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        SetOrientation();
    }


    private void SetOrientation()
    {
        
        if(_rb.velocity.x < 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", true);
        }
        else if(_rb.velocity.x == 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", false);
        }
        else if(_rb.velocity.x > 0)
        {
            _anim.SetBool("Right", true);
            _anim.SetBool("Left", false);
        }


        if(_rb.velocity.y <0)
        {
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", true);
        }
        else if(_rb.velocity.y == 0)
        {
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", false);
        }
        else if(_rb.velocity.y > 0)
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
        /*if (collision.tag == "Damageable")      
            collision.GetComponentInParent<EnemyScript>().TakeDamage(Damage);     */  
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
        _rb.velocity = _movement;

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
        _swingSound.Play();
        _anim.SetTrigger("IsSwinging");
    }

    void OnDrawGizmosSelected()
    {    
       Gizmos.color = Color.magenta;
       Gizmos.DrawSphere((Vector2)transform.position + (Vector2.up * 8.1f), 0.5f);
    }


    #endregion 
}
