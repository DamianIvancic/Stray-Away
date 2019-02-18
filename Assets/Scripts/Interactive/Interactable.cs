using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public virtual void DoOnInteract()
    {}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {          
            GameManager._GM.Player.InteractSprite.SetActive(true);
            GameManager._GM.Player.interactableScript = this;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(GameManager._GM._gameState == GameManager.GameState.Playing)
        {
            if (collider.tag == "Player")
            {
                GameManager._GM.Player.InteractSprite.SetActive(true);
                GameManager._GM.Player.interactableScript = this;
            }
        }    
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {         
            GameManager._GM.Player.InteractSprite.SetActive(false);
            GameManager._GM.Player.interactableScript = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {        
            GameManager._GM.Player.InteractSprite.SetActive(true);
            GameManager._GM.Player.interactableScript = this;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager._GM._gameState == GameManager.GameState.Playing)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameManager._GM.Player.InteractSprite.SetActive(true);
                GameManager._GM.Player.interactableScript = this;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {        
            GameManager._GM.Player.InteractSprite.SetActive(false);
            GameManager._GM.Player.interactableScript = null;
        }
    }
}
