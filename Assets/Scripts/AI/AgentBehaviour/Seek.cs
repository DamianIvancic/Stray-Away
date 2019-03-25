using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : AgentBehaviour
{
    public override Steering GetSteering() //a simple behaviour that moves the agent in direction of the target taking into account the agent's max acceleration
    {
        Steering steering = new Steering();
        steering.linearVelocity = target.transform.position - transform.position;
        steering.linearVelocity.Normalize();
        steering.linearVelocity = steering.linearVelocity * agent.maxAccel;
        return steering;
    }

}
