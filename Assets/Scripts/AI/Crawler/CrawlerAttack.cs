using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerAttack : State<Crawler>
{
    private static CrawlerAttack _instance;

    //states this can transit into
    private static CrawlerIdle _idleReference;
    private static CrawlerRetreat _retreatReference;

    public static CrawlerAttack Instance
    {
        get
        {
            if (_instance == null)
            {
                new CrawlerAttack();
            }

            return _instance;
        }
    }

    public CrawlerAttack()
    {
        if (_instance != null)
            return;
        _instance = this;
        _idleReference = CrawlerIdle.Instance;
        _retreatReference = CrawlerRetreat.Instance;
    }


    public override void EnterState(Crawler owner)
    {
        owner.destination = null;
        owner.readyToStrike = false;
        owner.stateFinished = false;

        Debug.Log("State enter: " + this);
    }

    public override void UpdateState(Crawler owner)
    {
        UpdateAI(owner);
        if (owner.stateMachine.currentState == this)
        {
            UpdateMovement(owner);
            UpdateAnimator(owner);
        }
    }

    public override void UpdateAI(Crawler owner)
    {
        Vector2 playerPosition = GameManager.GM.Player.transform.position;

        if (owner.readyToStrike == false)
        {
            if (owner.destination == null || !owner.nav2D.CalculatePath((Vector2)owner.destination, owner.nav2D.path)) //just use nav2D to check that the position isn't unreachable
            {
                Vector2 direction = (playerPosition - (Vector2)owner.transform.position) * 0.8f;
                direction = Quaternion.AngleAxis(Random.Range(-60, 61), Vector3.forward) * direction;

                owner.destination = (Vector2)owner.transform.position + direction;
            }
            if (((Vector2)owner.destination - (Vector2)owner.transform.position).magnitude < 0.5f || GameManager.GM.Player.RB.velocity.magnitude > owner.velocity.magnitude)
            {
                owner.readyToStrike = true;
            }
        }

        if (owner.readyToStrike)
            owner.destination = playerPosition;

        if (owner.stateFinished)
            owner.stateMachine.ChangeState(CrawlerRetreat.Instance);

        if (owner.aggro == false)
            owner.stateMachine.ChangeState(CrawlerIdle.Instance);
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
