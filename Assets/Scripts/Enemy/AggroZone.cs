using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroZone : MonoBehaviour
{
    private Enemy _owner;
    private Transform _transform;

	void Start ()
    {
        _owner = transform.parent.GetComponentInChildren<Enemy>();
        _transform = transform;
	}
	
    void Update()
    {
        _transform.position = _owner.transform.position;
    }

	void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.tag == "Player")
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
