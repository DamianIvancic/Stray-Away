using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerSwing : StateMachineBehaviour {

    private Crawler CrawlerScript;

    private bool ready; //need a bool else damage gets inflicted multiple times from 0.9 to 1.0 

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CrawlerScript = animator.gameObject.GetComponentInChildren<Crawler>();
        CrawlerScript.swinging = true;
        ready = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime > 0.9f && ready == true)
        {
            if (CrawlerScript.playerInRange)
            {
                HealthManager.Instance.TakeDamage(0);
                HealthManager.OnDamageTakenCallback.Invoke();
                ready = false;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CrawlerScript.swinging = false;
        CrawlerScript.destination = null;
        CrawlerScript.SetState(Crawler.State.Retreat);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
