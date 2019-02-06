using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    
    public float Speed = 5;
    public int Health = 2;
    public bool Stunned = false;
    public bool IsDead = false;

    public Transform Player;

    public float InvulTimer = 1;
    public float InvulMax = 1;

    private Animator _anim;
    private Transform _transform;

    private Vector3 _distance;

    private AudioSource MonsterSound;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _transform = transform;
        MonsterSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player = collision.transform;
            MonsterSound.Play();
        }
    }

    // Update is called once per frame
    void Update ()
    {

        InvulTimer += Time.deltaTime;


        if (Player && !IsDead && !Stunned)
        {
            if (Vector3.Distance(Player.position, transform.position) > 1.2f || Vector3.Distance(Player.position, transform.position) < -1.2f)
            {

                float movingAngle = Vector3.SignedAngle(Player.position - transform.position, Vector3.right, Vector3.forward);
                _anim.SetFloat("MovingAngle", movingAngle);
                _anim.SetBool("IsWalking", true);

                MoveToward(Player.position);
            }
        }
    }

    public void MoveToward(Vector2 target)
    {
        float step = Speed * Time.deltaTime;

        // move sprite towards the target location
        transform.position = Vector2.MoveTowards(transform.position, target, step);
    }

    public void TakeDamage(int damage)
    {
        if (InvulTimer > InvulMax)
        {
            InvulTimer = 0;

            Stunned = true;
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
        Stunned = false;
        _anim.SetBool("IsWalking", true);
    }

    public void Death()
    {
        _anim.SetTrigger("Death");
        IsDead = true;
        Destroy(gameObject, 5);
    }

    
    public void PrintDamage(int damage)
    {
        Debug.Log("I was hit for: " + damage + " Damage");
    }

}
