using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : Enemy {

    private Transform _player;
    private Rigidbody2D _playerRB;

    [HideInInspector]
    public StateMachine<Crawler> stateMachine;
    [HideInInspector]
    public NavMeshAgent2D nav2D; //only used to calculate if the destination is reachable, not for the movement itself since it produces sliding
    [HideInInspector]
    public Animator anim;
    private AudioSource _damagedSound;
    private BoxCollider2D _trigger;
    private CircleCollider2D _collider;
    private Rigidbody2D _RB;

    [HideInInspector]
    public Vector2 startingPos;
    [HideInInspector]
    public Vector2? destination = null;
    [HideInInspector]
    public Vector2 velocity; //movement is based on standard velocity/direction translations except when using NavMesh2D to return to the spawn position

    private int _healthPoints = 2;
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
    [HideInInspector]
    public bool aggro = false;
    [HideInInspector]
    public float movingAngle;
    [HideInInspector]
    public float speed = 6f;
    [HideInInspector]
    public float aggroRadius; //if the crawler gets further away than this from the startPos aggro is reset. also half of this is the radius of the area where the crawler can walk around in idle state
    private float _strikeTimer = 0f; //measures when enough times has passed between attacks

 


    void Awake()
    {
        stateMachine = new StateMachine<Crawler>(this);
        stateMachine.ChangeState(CrawlerIdle.Instance);
        nav2D = GetComponent<NavMeshAgent2D>();
        anim = GetComponent<Animator>();
        _damagedSound = GetComponent<AudioSource>();
        _trigger = GetComponent<BoxCollider2D>();
        _collider = GetComponentInChildren<CircleCollider2D>();
        _RB = GetComponent<Rigidbody2D>();
        startingPos = transform.position;

        GameObject aggroZone = transform.parent.GetComponentInChildren<AggroZone>().gameObject;
        aggroRadius = aggroZone.GetComponent<CircleCollider2D>().radius;
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
            if(_healthPoints > 0)
            {
                if (((Vector2)transform.position - startingPos).magnitude > aggroRadius)
                    aggro = false;
    
                stateMachine.Update();
                _strikeTimer += Time.deltaTime;
            }        
        }
    }

    public override void SetAggro(bool state)
    {
        aggro = state;
    }

    public override void TakeDamage(int damage = 1)
    {
        _healthPoints -= damage;
        _damagedSound.Play();

        if (_healthPoints == 0)
        {
            anim.SetTrigger("Death");
            _collider.enabled = false;
            Destroy(gameObject.transform.parent.gameObject, 2f);
        }
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

            if (_strikeTimer >= 2f)
            {
                anim.SetTrigger("IsSwinging");
                _strikeTimer = 0f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        playerInRange = false;
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

        if (stateMachine.currentState == validState)
            destination = null;
    }

}
