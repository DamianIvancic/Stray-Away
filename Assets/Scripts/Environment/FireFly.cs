using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFly : MonoBehaviour
{
    public GameObject IntensiveLight;

    [HideInInspector]
    public StateMachine<FireFly> stateMachine;
    [HideInInspector]
    public Vector2 scale;
    [HideInInspector]
    public Vector2 startingPos;
    [HideInInspector]
    public Vector2 destination;
    [HideInInspector]
    public Vector2 movementDirection;

    void Start()
    {
        startingPos = transform.position;

        stateMachine = new StateMachine<FireFly>(this);
        stateMachine.ChangeState(FireFlyIncreasing.Instance);     
    }
	
	void Update ()
    {
        stateMachine.Update();
	}


    void OnDrawGizmosSelected()
    {
        if (destination != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(destination, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(startingPos, 0.5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector2(0,0), 0.5f);        
            Gizmos.DrawSphere(new Vector2(1,0), 0.5f);
        }
    }
}
