using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour {

    private Transform _transform;
    private Camera _mainCam;

	void Start()
    {
        _transform = transform;
        _mainCam = Camera.main;
    }

	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            transform.position = pos;
        }
	}
}
