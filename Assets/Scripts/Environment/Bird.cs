using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public enum State
    {
        Walk,
        Fly
    }

    private NavMeshAgent2D _nav2D;

    private AudioSource _audio;
    private Animator _anim;
    private CircleCollider2D _collider;
    private Transform _transform;

    private Vector2 _velocity;
    private Vector2? _destination;

    private State _currentState = State.Walk;

	
	void Start ()
    {
        _nav2D = GetComponent<NavMeshAgent2D>();
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _collider = GetComponent<CircleCollider2D>();
        _transform = GetComponent<Transform>();
	}

	void Update ()
    {
        if(_currentState == State.Walk)
        {     
            _destination = (Vector2)_transform.position + Random.insideUnitCircle * 3;

            if (_nav2D.CalculatePath((Vector2)_destination))
                _nav2D.destination = (Vector2)_destination;      
        }
        else if(_currentState == State.Fly)
            _transform.Translate(_velocity * 10 * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        Vector3 Scale = _transform.localScale;

        if (_velocity.x > 0)
            Scale.x = 1.5f;
        else if (_velocity.x < 0)
            Scale.x = -1.5f;

        _transform.localScale = Scale;
    }


    void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player" && trigger.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0f)
        {
            _nav2D.enabled = false;
            _collider.enabled = false;
            _velocity = _transform.position - trigger.gameObject.transform.position;
            _velocity.Normalize();

            _audio.Play();
            _anim.SetBool("FlyAway", true);
            _currentState = State.Fly;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_destination != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere((Vector3)_destination, 0.5f);
        }
    }
}
