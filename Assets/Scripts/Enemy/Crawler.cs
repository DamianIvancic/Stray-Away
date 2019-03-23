using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy
{
    public Transform Player;

    private Rigidbody2D _playerRB;

    private NavMeshAgent2D _nav2D; //only used to calculate if the destination is reachable, not for the movement itself since it produces sliding
    private Animator _anim;
    private BoxCollider2D _trigger;
    private Rigidbody2D _RB;

    [HideInInspector]
    public Vector2? destination = null;
    private Vector2 _velocity; //movement is based on standard velocity/direction translations except when using NavMesh2D to return to the spawn position

    [HideInInspector]
    public bool playerInRange = false; //set by trigger enter/exit, checked by animator script to see if it's ok to inflict damage
    [HideInInspector]
    public bool swinging = false; // gets set by the animator script to prevent moving during swinging animation
    private bool _cleared = false; // used to prevent the ClearTarget coroutine from being called multiple times
    private bool _readyToStrike = false; //used by SetTarget to determine if movement/position are ok to perform an attack
    private float _strikeTimer = 0f; //measures when enough times has passed between attacks
    private float _movingAngle;
    private float _speed;

    public enum State
    {
        Idle,
        Attack,
        Retreat,
        Return
    }

    private State? _currentState = State.Idle;


    void Awake()
    {
        _playerRB = Player.GetComponent<Rigidbody2D>();

        _nav2D = GetComponent<NavMeshAgent2D>();
        _anim = GetComponent<Animator>();
        _trigger = GetComponent<BoxCollider2D>();
        _RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            UpdateAI();
            UpdateMovement();      
            UpdateAnimator();

            _strikeTimer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.L))
            Debug.Log(_currentState);
    }

   public override void UpdateAI()
    {
        switch (_currentState) //switching from State.Idle to State.Attack and vice versa is done with the help of the AggroZone script on an object attached to the same parent
        {
            case (State.Idle):
                if (destination == null || !_nav2D.CalculatePath((Vector2)destination)) //just use nav2D to check that the position isn't unreachable
                {
                    destination = (Vector2) transform.position + (Random.insideUnitCircle * Random.Range(5, 10));
                    _cleared = false;
                }
                if (destination != null && ((Vector2)destination - (Vector2)transform.position).magnitude < 0.5f && _cleared == false)
                {
                    StartCoroutine(ClearTarget(2f, (State)_currentState)); //clears destination but only if the crawler is still in the same state
                }
                break;
            case (State.Attack): //doesn't head directly for the player but rather chooses a point from which to begin an attack and heads for the player once the point is reached
                if (_readyToStrike)
                {
                    destination = Player.position;
                }
                else
                {
                    if (destination == null || !_nav2D.CalculatePath((Vector2)destination)) //just use nav2D to check that the position isn't unreachable
                    {
                        Vector2 direction = (Player.position - transform.position) * 0.8f;                     
                        direction = Quaternion.AngleAxis(Random.Range(-60, 61), Vector3.forward) * direction;

                        destination = (Vector2)transform.position + direction;
                    }
                    if (((Vector2)destination - (Vector2)transform.position).magnitude < 0.5f || GameManager.GM.Player.RB.velocity.magnitude > _velocity.magnitude)
                    {
                        _readyToStrike = true;
                    }
                }
                break;
            case (State.Retreat):
                if (destination == null) //assign the a destination to which retreat to
                {
                    Vector2 direction = transform.position - Player.position;
                    direction.Normalize();
                    direction *= 5;

                    destination = (Vector2?)((Vector2)Player.position + direction);
                }
                if (destination != null && _nav2D.CalculatePath((Vector2)destination) == false) //if the destination is unreachable assign a new one
                {
                    destination = null;
                    break;
                }
                if (destination != null && ((Vector2)destination - (Vector2)transform.position).magnitude < 0.5f) //when the destination is reached switch back to attack
                {
                    destination = null;
                    _currentState = null;
                    _readyToStrike = false;                
                    StartCoroutine(SetState(0.5f, State.Attack));
                }
                if(_movingAngle != float.NaN) //taking into account inverted movement animation quits retreat and begins attack if the player is behind the crawler's back
                {
                    if(_movingAngle > -45 && _movingAngle <= 45) 
                    {
                        if(Player.position.x <= transform.position.x)
                        {
                            destination = null;                      
                            _readyToStrike = false;
                            _currentState = State.Attack;
                        }
                    }
                    else if(_movingAngle > 45 && _movingAngle <= 135)
                    {
                        if (Player.position.y >= transform.position.y)
                        {
                            destination = null;
                            _readyToStrike = false;
                            _currentState = State.Attack;
                        }
                    }
                    if(_movingAngle > 135 || _movingAngle <= -135)
                    {
                        if (Player.position.x >= transform.position.x)
                        {
                            destination = null;
                            _readyToStrike = false;
                            _currentState = State.Attack;
                        }
                    }
                    if(_movingAngle > -135 && _movingAngle <= -45)
                    {
                        if (Player.position.y <= transform.position.y)
                        {
                            destination = null;
                            _readyToStrike = false;
                            _currentState = State.Attack;
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    public override void UpdateMovement()
    {
        if (_currentState == State.Retreat)
            _speed = 2;
        else
            _speed = 5;

        if (destination != null && ((Vector2)destination - (Vector2)transform.position).magnitude >= 0.5f && !swinging)
        {
            _velocity = ((Vector2)destination - (Vector2)transform.position).normalized * _speed * Time.deltaTime;
            transform.position += (Vector3)_velocity;
        }
        else
            _velocity = Vector2.zero;
    }

    public override void UpdateAnimator()
    {
         _movingAngle = float.NaN; //basically use NaN as if it were null

        if (_velocity.magnitude > 0f)
        {
            _movingAngle = Vector2.SignedAngle(_velocity, Vector2.right);
       
            _anim.SetBool("IsWalking", true);
        }
        else
        {
            if (destination != null)      
                _movingAngle = Vector2.SignedAngle((Vector2)destination - (Vector2)transform.position, Vector2.right);              
            
            _anim.SetBool("IsWalking", false);
        }

        if(_movingAngle != float.NaN)
        {
            if (_currentState == State.Retreat) 
            {
                _movingAngle += 180; //makes it look like it's moving backwards
                if (_movingAngle > 180)
                    _movingAngle -= 360;
            }

            _anim.SetFloat("MovingAngle", _movingAngle);
        }
    }

    public override void SetAggro()
    {
        destination = null;
        _currentState = State.Attack;
    }

    public override void ResetAggro()
    {
        destination = null;
        _readyToStrike = false;
        _currentState = State.Idle;
    }

    #region Collisions  <----- collisions/triggers go here
    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.tag == "Player" && _strikeTimer >= 2f)
        {
            _anim.SetTrigger("IsSwinging");
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
                _anim.SetTrigger("IsSwinging");
                _strikeTimer = 0f;
            }
   
            float angleToTarget;
            if (destination != null)
            {
                angleToTarget = Vector2.SignedAngle((Vector2)destination - (Vector2)transform.position, Vector2.right);
                _anim.SetFloat("MovingAngle", angleToTarget);
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

    IEnumerator ClearTarget(float waitPeriod, State validState)
    {
        _cleared = true;
        yield return new WaitForSeconds(waitPeriod);

        if(_currentState == validState)
            destination = null;
    }

    public void SetState(State newState)
    {
        _currentState = newState;
    }

    public IEnumerator SetState(float waitPeriod, State newState)
    {
        yield return new WaitForSeconds(waitPeriod);
        _currentState = newState;
    }

}