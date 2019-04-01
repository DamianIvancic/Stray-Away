using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyDecreasing : State<FireFly>
{
    private static FireFlyDecreasing _instance;

    [Header("States this can transitions into")]
    private static FireFlyIncreasing _increasingStateReference;

    public static FireFlyDecreasing Instance
    {
        get
        {
            if (_instance == null)
            {
                new FireFlyDecreasing();
            }

            return _instance;
        }
    }

    public FireFlyDecreasing()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void EnterState(FireFly owner)
    {

        owner.destination = owner.startingPos;
        owner.movementDirection = (Vector2)owner.destination - (Vector2)owner.transform.position;
        owner.movementDirection.Normalize();
    }
    public override void UpdateState(FireFly owner)
    {
        UpdateAI(owner);
        UpdateMovement(owner);
    }
    public override void UpdateAI(FireFly owner)
    {
        Vector2 scale = owner.IntensiveLight.transform.localScale;
        scale.x -= 0.02f;
        scale.y -= 0.02f;
        owner.IntensiveLight.transform.localScale = scale;

        if (scale.x <= 0.1 && scale.y <= 0.1)
            owner.stateMachine.ChangeState(FireFlyIncreasing.Instance);
    }
    public override void UpdateMovement(FireFly owner)
    {
        owner.transform.position += (Vector3)owner.movementDirection * Time.deltaTime*2;
    }
    public override void UpdateAnimator(FireFly owner)
    {

    }
    public override void ExitState(FireFly owner)
    {
    }
}
