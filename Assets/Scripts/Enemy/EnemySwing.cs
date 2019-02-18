using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwing : MonoBehaviour {

    public int Damage = 1;

    [HideInInspector]
    public bool _playerInRange = false;

    private Animator _anim;

    void Start()
    {
        _anim = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            _playerInRange = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && GameManager._GM._gameState == GameManager.GameState.Playing)
            _anim.SetTrigger("IsSwinging");           
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _anim.SetTrigger("IsSwinging");
            _playerInRange = false;        
        }
    }
}
