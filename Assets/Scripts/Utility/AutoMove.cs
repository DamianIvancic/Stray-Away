using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour {

    private Rigidbody2D _rb;
    private Animator _anim;
    private Vector3? _targetPos;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _targetPos = null;
    }

    void Update()
    {
        if (_targetPos != null)
        {
            Vector3 Direction = (Vector3)_targetPos - transform.position;
            if (Direction.magnitude < 1f)
            {
                _targetPos = null;
                _rb.velocity = Vector3.zero;
            }
            else
            {
                Direction.Normalize();
                _rb.velocity = Direction;
            }

            UpdateAnimator();
        }
    }

    void UpdateAnimator()
    {
        if (_rb.velocity.magnitude > 0f)
            _anim.SetBool("IsMoving", true);
        else
            _anim.SetBool("IsMoving", false);

        if (_rb.velocity.x < 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", true);
        }
        else if (_rb.velocity.x == 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", false);
        }
        else if (_rb.velocity.x > 0)
        {
            _anim.SetBool("Right", true);
            _anim.SetBool("Left", false);
        }


        if (_rb.velocity.y < 0)
        {
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", true);
        }
        else if (_rb.velocity.y == 0)
        {
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", false);
        }
        else if (_rb.velocity.y > 0)
        {
            _anim.SetBool("Up", true);
            _anim.SetBool("Down", false);
        }
    }

    public void MoveUp()
    {
        _targetPos = transform.position + new Vector3(0, 3, 0);
    }

    public void MoveDown()
    {      
        _targetPos = transform.position + new Vector3(0, -3, 0);
    }

    public void MoveLeft()
    {       
        _targetPos = transform.position + new Vector3(-3, 0, 0);
    }

    public void MoveRight()
    {      
        _targetPos = transform.position + new Vector3(3, 0, 0);
    }
}
