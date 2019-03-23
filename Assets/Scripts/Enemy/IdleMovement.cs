using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMovement : MonoBehaviour
{
    public float Speed;

    private Animator _anim;

    private Vector2 _velocity;
    private Vector2 _startingPos;
    private Vector2? _destination = null;

    private bool _clear = false;

    void Start()
    {
        _startingPos = transform.position;

        _anim = GetComponent<Animator>();
    }

	void Update ()
    {
		if(_destination == null)
        {
            _destination = _startingPos + Random.insideUnitCircle * Random.Range(1, 15);
            if ((_startingPos - (Vector2)_destination).magnitude > 20f)
                _destination = null;

            _clear = false;
        }

        if(_destination != null)
        {      
            if (((Vector2)_destination - (Vector2)transform.position).magnitude >= 0.5f)
            {
                Vector2 direction = (Vector2)_destination - (Vector2)transform.position;
                direction.Normalize();

                _velocity = (Vector3)direction * Speed * Time.deltaTime;

                transform.position += (Vector3)_velocity;
            }


            if (((Vector2)_destination - (Vector2)transform.position).magnitude < 0.5f && !_clear)
            {              
                StartCoroutine(ClearDestination(2f));
                _velocity = Vector2.zero;
            }
        }

        CheckObstacles();
        UpdateAnimator();
	}

    void UpdateAnimator()
    {
        float movingAngle = float.NaN;

        if (_velocity.magnitude > 0f)
        {
            movingAngle = Vector2.SignedAngle(_velocity, Vector2.right);

            _anim.SetBool("IsWalking", true);
        }
        else
        {
            if (_destination != null)
                movingAngle = Vector2.SignedAngle((Vector2)_destination - (Vector2)transform.position, Vector2.right);

            _anim.SetBool("IsWalking", false);
        }

        if(movingAngle != float.NaN)
         _anim.SetFloat("MovingAngle", movingAngle);
    }

    void CheckObstacles()
    {
        if(_destination != null)
        {
            RaycastHit2D raycast;

            raycast = Physics2D.Raycast((Vector2)transform.position + _velocity *2f, (Vector2)_destination - (Vector2)transform.position, ((Vector2)_destination - (Vector2)transform.position).magnitude);
               
            if (raycast.transform != null && raycast.transform != transform)
            {
                Debug.Log(raycast.transform.name);
                _destination = null;
            }
        }
    }

    IEnumerator ClearDestination(float waitPeriod)
    {
        _clear = true;
        yield return new WaitForSeconds(waitPeriod);
        _destination = null;
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
