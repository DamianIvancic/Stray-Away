using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine <T> // the generic is used just to prevent states that are made for different types from being put on the wrong object
{                                //not a MonoBehaviour so the functionality of the states can be better integrated with state-independent functionalities of the owner class
    public T Owner;               

    [HideInInspector]
    public State<T> currentState;

    public StateMachine(T owner)
    {
        Owner = owner;
    }

    public void Update() //update has to explicity called by the owner class since this isn't a MonoBehaviour
    {    
        if(currentState != null)
         currentState.UpdateState(Owner);
    }


    public void ChangeState(State<T> newState) //transitions are defined within states themselves - this function is only called when initializing the machine or from various state updates
    {                                          //although the conditions for calling this are sometimes set from other scripts such as the animator scripts                                             
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
