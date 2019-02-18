using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            GameManager._GM.MainCam.ZoomEnabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameManager._GM.MainCam.ZoomEnabled = true;
        }
    }
}
