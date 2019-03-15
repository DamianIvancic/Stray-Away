using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering
{
    public float angularVelocity;
    public Vector3 linearVelocity;

    public Steering()
    {
        angularVelocity = 0.0f;
        linearVelocity = new Vector3();
    }
}
