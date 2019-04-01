using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlyIncreasing : State<FireFly>
{
    private static FireFlyIncreasing _instance;

    [Header("States this can transition into")]
    private static FireFlyDecreasing _decreasingStateReference;

    public static FireFlyIncreasing Instance
    {
        get
        {
            if (_instance == null)
            {
                new FireFlyIncreasing();
            }

            return _instance;
        }
    }

    public FireFlyIncreasing()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void EnterState(FireFly owner)
    {
    

        owner.destination = owner.startingPos + (Random.insideUnitCircle * 3);
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
        scale.x += 0.02f;
        scale.y += 0.02f;
        owner.IntensiveLight.transform.localScale = scale;

        if (scale.x >= 1 && scale.y >= 1)
            owner.stateMachine.ChangeState(FireFlyDecreasing.Instance);
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
