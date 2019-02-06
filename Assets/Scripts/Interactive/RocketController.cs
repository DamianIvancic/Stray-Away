using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : Interactable {

    public SiloController SiloController;
    public GameObject RocketChoiceCollider;

    private Animator _anim;
    private BoxCollider2D _bc2d;




    private void Start()
    {
        _anim = GetComponent<Animator>();
        _bc2d = GetComponent<BoxCollider2D>();
    }

    public override void DoOnInteract()
    {
        base.DoOnInteract();

        _anim.SetTrigger("RocketRise");
        _bc2d.enabled = false;
        RocketChoiceCollider.SetActive(true);
        Invoke("SendCloseSignal", 7);
    }

    private void SendCloseSignal()
    {
        SiloController.CloseSilo();
    }
}
