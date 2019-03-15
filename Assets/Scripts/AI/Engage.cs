using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engage : Arrive
{

    public float AggroRadius;
    [HideInInspector]
    public Vector3? advancePoint = null;
    protected GameObject targetAux;

    public override void Awake()
    {
        base.Awake();

        targetAux = target;
        target = new GameObject();
    }

    public override Steering GetSteering()
    {
        SetAdvancePoint();

        if (advancePoint != null)
        {
            target.transform.position = (Vector3)advancePoint;
            if ((target.transform.position - transform.position).magnitude < 1f)
                Strike();
        }

        return base.GetSteering();
    }

    void SetAdvancePoint()
    {
        if (advancePoint == null && (targetAux.transform.position - transform.position).magnitude < AggroRadius)
        {
            advancePoint = targetAux.transform.position + (Vector3)Random.insideUnitCircle.normalized * 10;
        }
    }

    public virtual void Strike()
    {
        advancePoint = targetAux.transform.position;
        if (((Vector3)advancePoint - transform.position).magnitude < 1.5f)
            advancePoint = null;
    }
}
