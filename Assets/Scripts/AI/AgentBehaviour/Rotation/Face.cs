using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align //pay attention to the angle of the transform component, not to how the sprites look
{
    protected GameObject targetAux;

    public override void Awake()
    {
        base.Awake();

        targetAux = target;
        target = new GameObject();
        target.AddComponent<Agent>();
    }

    void OnDestroy()  
    {
        Destroy(target);
    }

    public override Steering GetSteering()
    {
        Vector3 direction = targetAux.transform.position - transform.position;
        if(direction.magnitude > 0.0f)
        {
            float targetOrientation = Mathf.Atan2(direction.y, direction.x);
            targetOrientation *= Mathf.Rad2Deg;
            target.GetComponent<Agent>().orientation = targetOrientation;
        }
        return base.GetSteering();
    }

}
