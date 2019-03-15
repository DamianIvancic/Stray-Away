using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : Arrive
{
    public FollowPath Path;
 
    public override void Awake()
    {
        base.Awake();
        target = new GameObject();

    }

    public override Steering GetSteering()
    {
        target.transform.position = Path.SetPosition(transform.position);

        return base.GetSteering();
    }
}
