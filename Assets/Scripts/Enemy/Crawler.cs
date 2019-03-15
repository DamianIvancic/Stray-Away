using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MonoBehaviour
{
    public Transform Player;
    public float AggroRadius;

    [HideInInspector]
    public Vector2? advancePoint = null;
    [HideInInspector]
    public bool _playerInRange = false; //set by trigger enter/exit, checked by animator script to see if it's ok to inflict damage
    [HideInInspector]
    public bool _swinging = false; // gets set by the animator script to prevent moving during swinging animation

    private Rigidbody2D _playerRB;

    private NavMeshAgent2D _nav2D;
    private Animator _anim;
    private BoxCollider2D _trigger;
    private Rigidbody2D _RB;

    private Vector2? _target = null;
    private bool _readyToStrike = false; //used by SetTarget to determine if movement/position are ok to perform an attack
    private float _strikeTimer = 0f; //measures when enough times has passed between attacks

    bool logged;

    public enum State
    {
        Idle,
        Attack,
        Retreat
    }
    [HideInInspector]
    public State _currentState = State.Idle;

    private Vector2? nDir;

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
            SetTarget();

            if (_swinging)
                _nav2D.destination = transform.position;
            else if (_target != null)
            {
                Vector2 targetAux = (Vector2)_target;
                _nav2D.destination = targetAux;
            }

            _strikeTimer += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.L))
                Debug.Log(_currentState);

            UpdateAnimator();
        }
    }

    void UpdateAnimator()
    {
        if (_nav2D.velocity.magnitude > 0f)
        {
            float movementAngle = Vector2.SignedAngle(_nav2D.velocity, Vector2.right);
            if (_currentState == State.Retreat)
            {
                movementAngle += 180;
                if (movementAngle > 180)
                    movementAngle -= 360;
            }

            _anim.SetFloat("MovingAngle", movementAngle);
            _anim.SetBool("IsWalking", true);
        }
        else
        {
            float angleToTarget;
            if (_target != null)
            {
                angleToTarget = Vector2.SignedAngle((Vector2)_target - (Vector2)transform.position, Vector2.right);
                _anim.SetFloat("MovingAngle", angleToTarget);
            }
            _anim.SetBool("IsWalking", false);
        }
    }

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
            _playerInRange = true;
    
            if(_strikeTimer >= 2f)
            {
                _anim.SetTrigger("IsSwinging");
                _strikeTimer -= 2f;
            }
   
            float angleToTarget;
            if (_target != null)
            {
                angleToTarget = Vector2.SignedAngle((Vector2)_target - (Vector2)transform.position, Vector2.right);
                _anim.SetFloat("MovingAngle", angleToTarget);
            }           
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.tag == "Player")
        {
            _playerInRange = false;
            _strikeTimer = 0f;          
        }
    }

    void SetTarget()
    {
        switch(_currentState)
        {
            case (State.Idle):
                if ((Player.position - transform.position).magnitude < AggroRadius) _currentState = State.Attack;
                break;
            case (State.Attack):
                if (_readyToStrike)
                {
                    _target = Player.position;
                   
                }
                else
                {
                    if ((advancePoint == null) || !_nav2D.CalculatePath((Vector2)(Player.position + advancePoint))) advancePoint = (Vector2?)Random.insideUnitCircle.normalized * 5;

                    _target = Player.position + advancePoint;
          
                    if (((Vector3)_target - transform.position).magnitude < 1f) _readyToStrike = true;
                }
                break;
            case (State.Retreat):            
                if(advancePoint == null)
                {
                    _nav2D.acceleration = 2;

                    Vector2 direction = transform.position - Player.position;
                    direction.Normalize();
                    direction *= 5;
                              
                    advancePoint = (Vector2?)((Vector2)Player.position + direction);
                }
                if (_nav2D.CalculatePath((Vector2)advancePoint))
                {                    
                    _target = advancePoint;              
                }
                else
                {
                    advancePoint = null;
                    _readyToStrike = false;
                    _currentState = State.Attack;
                    break;
                }

                if(((Vector2)_target - (Vector2)transform.position).magnitude < 0.2f)
                {
                    advancePoint = null;
                    _readyToStrike = false;
                    _nav2D.acceleration = 8;
                    _currentState = State.Attack;
                }
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_target != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere((Vector3)_target, 0.5f);
        }

        Gizmos.DrawWireSphere(transform.position, AggroRadius);
    }
}