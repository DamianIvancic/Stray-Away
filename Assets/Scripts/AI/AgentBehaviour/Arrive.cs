using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : AgentBehaviour
{
    public float targetRadius;
    public float slowdownRadius;
    public float timeToTarget = 0.1f;

    public override void Update()
    {
        agent.SetSteering(GetSteering(), weight);
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        Vector3 direction = target.transform.position - transform.position;
        float distance = direction.magnitude;

        float speed;
        if (distance < targetRadius)
            return steering;
        if (distance > slowdownRadius) //move at max speed while far enough away, then start gradually slowing down after a certain point the closer you get to the target
            speed = agent.maxSpeed;
        else
            speed = agent.maxSpeed * distance / slowdownRadius;

        Vector3 desiredVelocity = direction;
        desiredVelocity.Normalize();
        desiredVelocity*= speed;

        steering.linearVelocity = desiredVelocity - agent.velocity; //since agent velocity is calculated by multiplying steering.linearVelocity by Time.deltaTime 
        steering.linearVelocity /= timeToTarget;                          //it's always going to be lower than the velocity calculated here
        if(steering.linearVelocity.magnitude > agent.maxAccel)
        {
            steering.linearVelocity.Normalize();
            steering.linearVelocity *= agent.maxAccel;
        }

        return steering;
    }

   /* public float movementSpeed;

    void OnDrawGizmosSelected()
    {

        movementSpeed = agent.velocity.magnitude;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target.transform.position, slowdownRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(target.transform.position, targetRadius);
    }*/
}
