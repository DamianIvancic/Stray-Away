using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    public abstract void EnterState(T owner);
    public abstract void UpdateState(T owner);
    public abstract void UpdateAI(T owner);
    public abstract void UpdateMovement(T owner);
    public abstract void UpdateAnimator(T owner);
    public abstract void ExitState(T owner);
}
