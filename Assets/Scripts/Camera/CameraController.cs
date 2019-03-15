using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Read only")]
    public float Zoom = 0;
    public bool ZoomEnabled;

    private Transform Player;
    private GameObject _target;

    private Transform _camTransform;
    private Camera _camera;
 
	void Start () {

        Player = GameManager.GM.Player.transform;
        _camTransform = gameObject.transform;
        _camera = GetComponent<Camera>();

        ZoomEnabled = true;
	}

    void Update()
    {
        Vector3 Pos = Player.position;
        Pos.z -= 10;
        _camTransform.position = Pos;

        if (ZoomEnabled)
        {
            Zoom = Mathf.Clamp(_camera.orthographicSize + Input.GetAxis("Mouse ScrollWheel") * -20, 10f, 25);
            _camera.orthographicSize = Zoom;
        }
        else
            _camera.orthographicSize = 10;
    }  
}
