using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform Player;

    private GameObject _target;

    private Transform _camTransform;
    private Camera _camera;
   // private float _zoom = 0;

    private enum Mode
    {
        FollowPlayer,
        TargetObject
    }

    private Mode _cameraMode;
 
    void Awake()
    {
        Player = FindObjectOfType<PlayerController>().gameObject.transform;
    }

	void Start () {

        _camTransform = gameObject.transform;
        _camera = GetComponent<Camera>();

        _cameraMode = Mode.FollowPlayer;
	}

    void Update()
    {


        /*if (_cameraMode == Mode.FollowPlayer)
        {*/
            Vector3 Pos = Player.position;
            Pos.z -= 10;

            _camTransform.position = Pos;
            float size = Mathf.Clamp(_camera.orthographicSize + Input.GetAxis("Mouse ScrollWheel") * -20, 7.5f, 25);
            _camera.orthographicSize = size;
       // }
        /*else if (_cameraMode == Mode.TargetObject)
        {
            Vector2 Pos = new Vector3(_transform.position.x, transform.position.y);
            Vector2 Target = new Vector3(_target.transform.position.x, _target.transform.position.y);
            Vector2 move = Vector2.MoveTowards(Pos, Target, 0.3f);
            _transform.position = new Vector3(move.x, move.y, -10);

            Vector3 target3D = new Vector3(Target.x, Target.y, 0);
            float distance = (target3D - transform.position).magnitude;

            if (distance <= 15f)
                SetTarget(Player.gameObject);
        }*/
    }
    
    public void SetTarget(GameObject target)
    {
        Debug.Log("setting target: " + target.name);
        _target = target;

        if (_target == Player)
            _cameraMode = Mode.FollowPlayer;
        else
            _cameraMode = Mode.TargetObject;
    }
   
}
