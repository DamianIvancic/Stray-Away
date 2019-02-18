using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {


    public float MaxSpeed = 5;
    public float Accel = 2;
    public int Health = 2;

    public BoxCollider2D Collider;
    public AudioSource DamagedSound;

    public delegate void OnDeath(GameObject dead);
    public static OnDeath OnDeathCallback;

    private Transform _player;
    private PlayerController _playerController;

    private Transform _transform;
    private Animator _anim;
    private AudioSource _monsterSound;

    private float _invulTimer = 1;
    private float _invulPeriod = 1;

    private Vector2 _direction;
    private Vector2 _velocity;
    private Vector2 _initialPos;

    [HideInInspector]
    public bool _stunned = false;
    [HideInInspector]
    public bool _dead = false;

 
    private Vector2 Position;
    private Vector2 Target;
    private Vector2 DirectionA;
    bool rayhit = false;
    bool adjusted = false;
  

    void Start()
    {     
        _velocity = Vector2.zero;

        _transform = transform;
        _anim = GetComponentInChildren<Animator>();  
        _monsterSound = GetComponent<AudioSource>();

        _initialPos = transform.position;
    }


    void Update()
    {
        if (GameManager._GM._gameState == GameManager.GameState.Playing)
        {
            _invulTimer += Time.deltaTime;

            if (_velocity.magnitude > 0f)
            {
                Vector2 Movement = _velocity * Time.deltaTime;
                _transform.Translate(Movement, Space.World);

                Vector3 TargetPos = _transform.position + (Vector3)Movement;
                float movingAngle = Vector3.SignedAngle(TargetPos - _transform.position, Vector3.right, Vector3.forward);
                _anim.SetFloat("MovingAngle", movingAngle);
                _anim.SetBool("IsWalking", true);
            }
            else
                _anim.SetBool("IsWalking", false);

            if (!_dead && !_stunned)
            {
                if (_player && (Vector2.Distance(_player.position, _transform.position) > 1.2f || Vector2.Distance(_player.position, _transform.position) < -1.2f))
                    Seek(_player.position);
                else
                    Seek(_initialPos);
            }
            else
                _velocity = Vector2.zero;
        }
        else
        {
            _velocity = Vector2.zero;
            _anim.SetBool("IsWalking", false);
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            _player = collider.transform;
            _playerController = collider.gameObject.GetComponent<PlayerController>();
            _monsterSound.Play();
        }
    }


  /*  void AvoidWall()
    {
        float RaycastLength = 1f;
      
        Position = transform.position;
     
        RaycastHit2D hit; 
        hit = Physics2D.Raycast(Position + _velocity.normalized * 2f, _velocity.normalized, RaycastLength);
        if (hit.collider != null)
        {

            if (hit.collider != lastHit)
                adjusted = false;

            if (adjusted == false)
            {

                lastHit = hit.collider;

                Sphere = hit.point;

                Vector2 ObjectScale = hit.collider.gameObject.transform.localScale;

                Position = transform.position;
                Vector2 PlayerPos = _player.position;

                Target = Position + (PlayerPos - Position) / 2;
                DirectionA = Target - (Vector2)hit.collider.gameObject.transform.position;
                Target = (Vector2)hit.collider.gameObject.transform.position + DirectionA * 2;
                Vector2 CurrentMovement = hit.collider.gameObject.transform.position - transform.position;
                Vector2 MoveToTarget = Target - (Vector2)transform.position;

                Debug.Log("Time1" + Time.deltaTime);

                if (Vector2.Angle(MoveToTarget, CurrentMovement) < 5f)
                {
                    adjusted = true;
                    Debug.Log("Time2" + Time.deltaTime);
                    Debug.Log("Adjustment");
                    Vector2 Adjustment = Target - (Vector2)transform.position;
                    Adjustment = Quaternion.Euler(0, 0, 90) * Adjustment;
                    float root = Mathf.Sqrt(Adjustment.magnitude);
                    Target = Target + Adjustment;//root;
                }

                rayhit = true;
            }
        }

        if(rayhit)
        {       
            Seek(Target);
            float root = Mathf.Sqrt(DirectionA.magnitude);
            if ((Target - (Vector2)transform.position).magnitude <DirectionA.magnitude* root)
            {
                rayhit = false;
                adjusted = false;
            }
        }
    }*/

    void Seek(Vector2 targetPos)
    {     
        _direction = targetPos - (Vector2)_transform.position;
        float distance = _direction.magnitude;

        if (distance > 0.5f)
        {
            _direction.Normalize();

            _velocity += _direction * Accel;

            if (_velocity.magnitude > MaxSpeed)
            {
                _velocity.Normalize();
                _velocity *= MaxSpeed;
            }
        }
        else
            _velocity = Vector2.zero;
    }

  
    public void Return()
    {
        _player = null;
    }
   
    public void TakeDamage(int damage)
    {
        if (_invulTimer > _invulPeriod)
        {
            DamagedSound.Play();

            _invulTimer = 0;

            _stunned = true;
            _anim.SetBool("IsWalking", false);

            Invoke("UnStun", 1);

            Health -= damage;
            if (Health <= 0)
            {
                Death();
            }
        }
    }

    public void UnStun()
    {
        _stunned = false;
        _anim.SetBool("IsWalking", true);
    }

    public void Death()
    {
        _dead = true;

        Collider.enabled = false;
        _anim.SetTrigger("Death");
      
        OnDeathCallback.Invoke(gameObject);
        Destroy(gameObject, 2.5f);
    }
  
    public void PrintDamage(int damage)
    {
        Debug.Log("I was hit for: " + damage + " Damage");
    }

}
