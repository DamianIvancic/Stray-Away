using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy
{

    private Transform _player;
    private Rigidbody2D _playerRB;

    [HideInInspector]
    public StateMachine<Crawler> stateMachine;
    [HideInInspector]
    public NavMeshAgent2D nav2D; //only used to calculate if the destination is reachable, not for the movement itself since it produces sliding
    [HideInInspector]
    public Animator anim;
    private BoxCollider2D _trigger;
    private Rigidbody2D _RB;
    
    [HideInInspector]
    public Vector2? destination = null;
    [HideInInspector]
    public Vector2 velocity; //movement is based on standard velocity/direction translations except when using NavMesh2D to return to the spawn position

    [HideInInspector]
    public bool playerInRange = false; //set by trigger enter/exit, checked by animator script to see if it's ok to inflict damage
    [HideInInspector]
    public bool swinging = false; // gets set by the CrawlerSwing animator script to prevent moving during swinging animation
    [HideInInspector]
    public bool cleared = false; // used to prevent the ClearTarget coroutine from being called multiple times
    [HideInInspector]
    public bool readyToStrike = false; //used by SetTarget to determine if movement/position are ok to perform an attack
    [HideInInspector]
    public bool stateFinished; //used as a check for transitioning between some states. set by the states updates but also the animator script CrawlerSwing when the animation ends
    private float _strikeTimer = 0f; //measures when enough times has passed between attacks
    [HideInInspector]
    public float movingAngle;
    [HideInInspector]
    public float speed = 5f;
    [HideInInspector]
    public bool aggro = false;


    void Awake()
    {  
        stateMachine = new StateMachine<Crawler>(this);
        stateMachine.ChangeState(CrawlerIdle.Instance);
        nav2D = GetComponent<NavMeshAgent2D>();     
        anim = GetComponent<Animator>();
        _trigger = GetComponent<BoxCollider2D>();
        _RB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _player = GameManager.GM.Player.transform;
        _playerRB = _player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            stateMachine.Update();      
            _strikeTimer += Time.deltaTime;
        }
    }

    /*public override void UpdateMovement()
    {
        if (stateMachine._currentState == CrawlerRetreat.Instance)
            _speed = 2;
        else
            _speed = 5;

        if (destination != null && ((Vector2)destination - (Vector2)transform.position).magnitude >= 0.5f && !swinging)
        {
            velocity = ((Vector2)destination - (Vector2)transform.position).normalized * _speed * Time.deltaTime;
            transform.position += (Vector3)velocity;
        }
        else
            velocity = Vector2.zero;
    }

    public override void UpdateAnimator()
    {
        movingAngle = float.NaN; //basically use NaN as if it were null

        if (velocity.magnitude > 0f)
        {
            movingAngle = Vector2.SignedAngle(velocity, Vector2.right);
       
            _anim.SetBool("IsWalking", true);
        }
        else
        {
            if (destination != null)      
                movingAngle = Vector2.SignedAngle((Vector2)destination - (Vector2)transform.position, Vector2.right);              
            
            _anim.SetBool("IsWalking", false);
        }

        if(movingAngle != float.NaN)
        {    
            if (stateMachine._currentState == CrawlerRetreat.Instance)
            {
                movingAngle += 180; //makes it look like it's moving backwards
                if (movingAngle > 180)
                    movingAngle -= 360;
            }

            _anim.SetFloat("MovingAngle", movingAngle);
        }
    }*/

    public override void SetAggro(bool state)
    {
         aggro = state;
    }

    #region Collisions  <----- collisions/triggers go here
    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.tag == "Player" && _strikeTimer >= 2f)
        {
            anim.SetTrigger("IsSwinging");
            _strikeTimer = 0f;
        }
    }

    void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.tag == "Player" && GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            playerInRange = true;
    
            if(_strikeTimer >= 2f)
            {
                anim.SetTrigger("IsSwinging");
                _strikeTimer = 0f;
            }     
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.tag == "Player")
        {
            playerInRange = false;        
        }
    }
    #endregion

  
    void OnDrawGizmosSelected()
    {
        if (destination != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere((Vector3)destination, 0.5f);
        }
    }

    public void StartCoroutine(System.Func<float, State<Crawler>, IEnumerator> coroutine, float waitPeriod, State<Crawler> validState)
    {
        StartCoroutine(coroutine(waitPeriod, validState));
    }

    public IEnumerator ClearTarget(float waitPeriod, State<Crawler> validState)
    {
        cleared = true;
        yield return new WaitForSeconds(waitPeriod);

        if(stateMachine.currentState == validState)
            destination = null;
    }

}