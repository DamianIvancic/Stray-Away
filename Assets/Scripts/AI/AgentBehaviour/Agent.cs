using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour //class representing a "agent" moving through space
{
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;

    public float orientation; //actual current angle
    public float rotation; // speed of rotation
    public Vector3 velocity;
    protected Steering steering; //an object containing linear and angular velocity that transfers data about them from AgentBehaviour to Agent
                                 //AgentBehaviour calculates the more complex and behaviour specific parts of movement which Agent then uses to update it's velocity/rotation
                                 //and translate/rotate the transform

	void Start ()
    {
        velocity = Vector3.zero;
        steering = new Steering();
	}
	
	public virtual void Update () //calculates rotation/movement based on updates to the velocity and rotation variables received from AgentBehaviour and applied in LateUpdate()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;

        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360f)
            orientation -= 360.0f;

        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.forward, orientation);
	}

    public virtual void LateUpdate() //applies values returned by AgentBehaviour inside the Steering object to velocity and rotation
    {
        velocity += steering.linearVelocity * Time.deltaTime;
        rotation += steering.angularVelocity * Time.deltaTime;

        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }

        if(steering.angularVelocity == 0.0f)
        {
            rotation = 0.0f;
        }

        if(steering.linearVelocity.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }

        steering = new Steering();
    }

    public void SetSteering(Steering steering)
    {
        this.steering = steering;
    }

    public void SetSteering(Steering steering, float weight)
    {
        this.steering.linearVelocity += (weight * steering.linearVelocity);
        this.steering.angularVelocity += (weight * steering.angularVelocity);
    }
}
