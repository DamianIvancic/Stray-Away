using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav2DMover : MonoBehaviour
{
    public Transform Target;

    private Camera mainCam;
    private NavMeshAgent2D agent;

    void Start()
    {
        mainCam = Camera.main;
        agent = GetComponent<NavMeshAgent2D>();
    }


	void Update ()
    {
        agent.destination = Target.position;       	
	}
}
