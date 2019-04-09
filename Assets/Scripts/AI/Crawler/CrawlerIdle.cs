using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerIdle : State<Crawler>
{
    private static CrawlerIdle _instance;

    //states this can transit into
    private static CrawlerAttack _attackReference;

    public static CrawlerIdle Instance
    {
        get
        {
            if (_instance == null)
            {
                new CrawlerIdle();
            }

            return _instance;
        }
    }

    public CrawlerIdle()
    {
        if (_instance != null)
            return;
        _instance = this;
        _attackReference = CrawlerAttack.Instance;
    }


    public override void EnterState(Crawler owner)
    {
        owner.destination = null;
        owner.stateFinished = false;
        Debug.Log("State enter: " + this);
    }

    public override void UpdateState(Crawler owner)
    {
        UpdateAI(owner);
        UpdateMovement(owner);
        if (owner.stateMachine.currentState == this)
            UpdateAnimator(owner);
    }

    public override void UpdateAI(Crawler owner)
    {
        if (owner.aggro)
            owner.stateMachine.ChangeState(CrawlerAttack.Instance);

        if (owner.destination == null || !owner.nav2D.CalculatePath((Vector2)owner.destination, owner.nav2D.path)) //just use nav2D to check that the position isn't unreachable
        {
            owner.destination = (Vector2)owner.startingPos + (Random.insideUnitCircle * Random.Range(1, owner.aggroRadius/2));
            owner.cleared = false;
        }
        if (owner.destination != null && ((Vector2)owner.destination - (Vector2)owner.transform.position).magnitude < 0.5f && owner.cleared == false)
        {
            owner.StartCoroutine(owner.ClearTarget, 2f, this); //clears destination but only if the crawler is still in the same state
        }
    }

    public override void UpdateMovement(Crawler owner)
    {
        if (owner.destination != null && ((Vector2)owner.destination - (Vector2)owner.transform.position).magnitude >= 0.5f && !owner.swinging)
        {
            owner.velocity = ((Vector2)owner.destination - (Vector2)owner.transform.position).normalized * owner.speed * Time.deltaTime;
            owner.transform.position += (Vector3)owner.velocity;
        }
        else
            owner.velocity = Vector2.zero;
    }

    public override void UpdateAnimator(Crawler owner)
    {
        owner.movingAngle = float.NaN; //basically use NaN as if it were null

        if (owner.velocity.magnitude > 0f)
        {
            owner.movingAngle = Vector2.SignedAngle(owner.velocity, Vector2.right);

            owner.anim.SetBool("IsWalking", true);
        }
        else
        {
            if (owner.destination != null)
                owner.movingAngle = Vector2.SignedAngle((Vector2)owner.destination - (Vector2)owner.transform.position, Vector2.right);

            owner.anim.SetBool("IsWalking", false);
        }

        if (owner.movingAngle != float.NaN)
            owner.anim.SetFloat("MovingAngle", owner.movingAngle);

    }

    public override void ExitState(Crawler owner)
    {
        Debug.Log("State exit: " + this);
    }

}

