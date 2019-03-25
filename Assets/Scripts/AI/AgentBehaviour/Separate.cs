using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separate : AgentBehaviour
{
    private CircleCollider2D _separationZone;

    private List<Transform> _nearbyObjects;

    public override void Awake()
    {
        _separationZone = GetComponent<CircleCollider2D>();
        _nearbyObjects = new List<Transform>();

        base.Awake();
    }

    public override void Update()
    {
        agent.SetSteering(GetSteering(), weight);
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Enemy")
        {
            _nearbyObjects.Add(trigger.gameObject.transform);
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Enemy")
        {
            _nearbyObjects.Remove(trigger.gameObject.transform);
        }
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        if(_nearbyObjects.Count > 0)
        {
            foreach(Transform nearby in _nearbyObjects)
            {
                Vector3 separationDirection = transform.position - nearby.position;
                steering.linearVelocity += separationDirection;
            }

            steering.linearVelocity.Normalize();
            steering.linearVelocity *= agent.maxAccel;
        }

        return steering;
    }
}
