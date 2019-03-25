using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : AgentBehaviour
{
    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        float targetOrientation = target.GetComponent<Agent>().orientation;
        float rotation = targetOrientation - agent.orientation;

        rotation = MapToRange(rotation); //clamps values over 360 and determines shorter direction of rotation (negative or positive)
        float rotationMag = Mathf.Abs(rotation); //determine whether to rotate at full speed, with a reduced speed or if it all depending on angle with target orientation
        if (rotationMag < targetRadius)
            return steering;

        float targetRotation;
        if (rotationMag > slowRadius)
            targetRotation = agent.maxRotation;
        else
            targetRotation = agent.maxRotation * rotationMag / slowRadius; //the rotation slows down gradually as it gets closer

        targetRotation *= rotation / rotationMag; //determines the direction of rotation

        steering.angularVelocity = targetRotation - agent.rotation;
        steering.angularVelocity /= timeToTarget;
        float angularAccel = Mathf.Abs(steering.angularVelocity); //angular accel since overall the operation represents change in angular velocity over one frame
        if(angularAccel > agent.maxAngularAccel)
        {
            steering.angularVelocity /= angularAccel;
            steering.angularVelocity *= agent.maxAngularAccel;
        }
        return steering;
    }

}
