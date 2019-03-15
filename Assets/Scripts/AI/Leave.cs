using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave : AgentBehaviour
{
    public float escapeRadius;
    public float dangerRadius;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        Vector3 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;

        float reduction;
        if (distance > dangerRadius)
            return steering;
        if (distance < escapeRadius)
            reduction = 0f;
        else
            reduction = distance / dangerRadius * agent.maxSpeed;

        float speed = agent.maxSpeed  - reduction;

        steering.linearVelocity = direction.normalized * speed;

        if(steering.linearVelocity.magnitude > agent.maxAccel)
        {
            steering.linearVelocity.Normalize();
            steering.linearVelocity *= agent.maxAccel;
        }

        return steering;
    }
}
