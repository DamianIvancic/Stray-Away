using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raindrop : MonoBehaviour {

    [HideInInspector]
    public Rigidbody2D RB;

    private float _proximity;
    private float _width;
    private float _height;

	void Awake()
    {
        RB = GetComponent<Rigidbody2D>();

        _proximity = Random.Range(0, 1.1f);
        _width = Mathf.Lerp(0.5f, 1, _proximity);
        _height = Mathf.Lerp(2, 10, _proximity);
        RB.gravityScale = Mathf.Lerp(0.1f, 3, _proximity);

        Vector3 scale = transform.localScale;
        scale.x *= _width;
        scale.y *= _height;
        transform.localScale = scale;
    }

    void Update()
    {
        RB.AddForce(new Vector2(-15f, 0));
    }
}
