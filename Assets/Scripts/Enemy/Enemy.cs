using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void UpdateAI();
    public abstract void UpdateMovement();
    public abstract void UpdateAnimator();
    public abstract void SetAggro();
    public abstract void ResetAggro();
}
