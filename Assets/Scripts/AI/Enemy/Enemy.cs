using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    public abstract void SetAggro(bool state);
    public abstract void TakeDamage(int damage = 1);
}
