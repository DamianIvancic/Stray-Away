using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : AgentBehaviour
{
    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        steering.linearVelocity = transform.position - target.transform.position;
        steering.linearVelocity.Normalize();
        steering.linearVelocity = steering.linearVelocity * agent.maxAccel;
        return steering;
    }

}
