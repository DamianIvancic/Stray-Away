using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{

    public T Owner;
    public State<T> currentState;

    public StateMachine(T owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        if(currentState != null)
         currentState.UpdateState(Owner);
    }


    public void ChangeState(State<T> newState)
    {
        if (currentState != null)
            currentState.ExitState(Owner);

        currentState = newState;
        currentState.EnterState(Owner);
    }

    public IEnumerator ChangeState(float waitPeriod, State<T> newState)
    {
        yield return new WaitForSeconds(waitPeriod);

        if (currentState != null)
            currentState.ExitState(Owner);

        currentState = newState;
        currentState.EnterState(Owner);
    }
}
