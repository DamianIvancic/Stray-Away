using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Vector2? nullable;
    public Vector2 vec;

	void Update ()
    {
        nullable = (Vector2?) Random.insideUnitCircle;
        vec = ((Vector2)nullable).normalized;
        vec *= 5;
        nullable = (Vector2?)vec;
        vec = (Vector2)nullable;
     
	}
}
