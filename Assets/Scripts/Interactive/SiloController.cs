using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiloController : Interactable
{

    public bool IsOpen = false;
    public BoxCollider2D rocketTrigger;
    public RocketController RocketController;

    public TextScroller TextScroller;

    public bool TextFinished = false;

    private BoxCollider2D _siloTrigger;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _siloTrigger = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (TextFinished && Input.anyKeyDown)
        {
            TextScroller.ScrollText("");
            TextFinishedToggle();
        }
    }

    public override void DoOnInteract()
    {
        if(!IsOpen)
        {
            base.DoOnInteract();

            _anim.SetBool("IsOpen", true);
            IsOpen = true;
            _anim.SetTrigger("RocketRise");
            TextScroller.ScrollText("This appears to be a rocket silo. Interesting...             " +
                "\nWHAT IS THIS. A giant meteor twice the size of the one that caused the calamity all those years back." +
                "\nLuckily this rocket can take us to another planet. It just needs fuel.");
            Invoke("TextFinishedToggle", 16);
            rocketTrigger.enabled = true;
            _siloTrigger.enabled = false;
        }
    }



    public void CloseSilo()
    {
        _anim.SetBool("IsOpen", false);
    }

    public void TextFinishedToggle()
    {
        TextFinished = !TextFinished;
    }
}
