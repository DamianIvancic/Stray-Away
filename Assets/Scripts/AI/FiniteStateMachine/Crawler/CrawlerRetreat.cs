using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerRetreat : State<Crawler>
{
    private static CrawlerRetreat _instance;

    //states this can transit into
    private static CrawlerAttack _attackReference;
    private static CrawlerIdle _idleReference;

    public static CrawlerRetreat Instance
    {
        get
        {
            if (_instance == null)
            {
                new CrawlerRetreat();
            }

            return _instance;
        }
    }

    public CrawlerRetreat()
    {
        if (_instance != null)
            return;
        _instance = this;
    }


    public override void EnterState(Crawler owner)
    {
        owner.speed = 2;
        owner.destination = null;
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

        if (owner.destination == null) //assign the a destination to which retreat to
        {
            Vector2 direction = (Vector2)owner.transform.position - playerPosition;
            direction.Normalize();
            direction *= 5;

            owner.destination = (Vector2?)((Vector2)playerPosition + direction);
        }
        if (owner.destination != null && owner.nav2D.CalculatePath((Vector2)owner.destination) == false) //if the destination is unreachable assign a new one
        {
            owner.destination = null;
        }
        if ((owner.destination != null && ((Vector2)owner.destination - (Vector2)owner.transform.position).magnitude < 0.5f) && owner.stateFinished == false)  //when the destination is reached switch back to attack
        {
            owner.stateFinished = true;
            owner.StartCoroutine(owner.stateMachine.ChangeState(0.5f, CrawlerAttack.Instance));
        }
        if (owner.movingAngle != float.NaN) //taking into account inverted movement animation quits retreat and begins attack if the player is behind the crawler's back
        {
            if (owner.movingAngle > -45 && owner.movingAngle <= 45)
            {
                if (playerPosition.x <= owner.transform.position.x)
                {
                    owner.stateMachine.ChangeState(CrawlerAttack.Instance);
                }
            }
            else if (owner.movingAngle > 45 && owner.movingAngle <= 135)
            {
                if (playerPosition.y >= owner.transform.position.y)
                {
                    owner.stateMachine.ChangeState(CrawlerAttack.Instance);
                }
            }
            if (owner.movingAngle > 135 || owner.movingAngle <= -135)
            {
                if (playerPosition.x >= owner.transform.position.x)
                {
                    owner.stateMachine.ChangeState(CrawlerAttack.Instance);
                }
            }
            if (owner.movingAngle > -135 && owner.movingAngle <= -45)
            {
                if (playerPosition.y <= owner.transform.position.y)
                {
                    owner.stateMachine.ChangeState(CrawlerAttack.Instance);
                }
            }
        }

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
        {
            owner.movingAngle += 180; //makes it look like it's moving backwards
            if (owner.movingAngle > 180)
                owner.movingAngle -= 360;
            
            owner.anim.SetFloat("MovingAngle", owner.movingAngle);
        }
    }

    public override void ExitState(Crawler owner)
    {
        owner.speed = 5;

        Debug.Log("State exit: " + this);
    }
}
