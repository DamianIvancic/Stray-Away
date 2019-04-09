using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroZone : MonoBehaviour
{
    private Enemy _owner;
   
    void Start()
    {
        _owner = transform.parent.GetComponentInChildren<Enemy>();
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.tag == "Player")
        {
            _owner.SetAggro(true);
        }
    }

    void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.tag == "Player")
        {
            _owner.SetAggro(true);
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.tag == "Player")
        {
            _owner.SetAggro(false);
        }
    }
}
