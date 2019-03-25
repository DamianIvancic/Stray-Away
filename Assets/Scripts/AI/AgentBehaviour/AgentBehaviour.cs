using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviour : MonoBehaviour
{
    public float weight = 1.0f;

    public GameObject target;
    protected Agent agent;

    public virtual void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
    }

    public virtual void Update()
    {
        agent.SetSteering(GetSteering());
    }

    public virtual Steering GetSteering()
    {
        return new Steering();
    }

    public float MapToRange(float rotation)
    {
        rotation %= 360.0f; //only used to clamp values that are above 360

        if(Mathf.Abs(rotation) > 180.0f) //only calculated this way so there would be a smaller number of total angles to rotate  (for example 165 instead of -195)
        {
            if (rotation < 0.0f)       
                rotation += 360.0f;   
            else
                rotation -= 360.0f;
        }
        return rotation;
    }

    public Vector3 OrientationToVector(float orientation)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f; //x is normally cos(orientation) * vector.mag but we're only interested in direction so mag is 1
        vector.y = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;

        return vector.normalized;
    }
}
