using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

    private SpriteRenderer _sprite;
    private Color _color;

    private bool _increasing = false;

	void Start () {

        _sprite = GetComponent<SpriteRenderer>();
        _color = _sprite.color;
	}
	
	void Update ()
    {
        if (_color.a == 1)
            _increasing = false;
        else if (_color.a <= 0.4)
            _increasing = true;

        if(_increasing)   
            _color.a+= 0.005f;         
        else      
            _color.a-= 0.005f;

        _sprite.color = _color;
	}
}
