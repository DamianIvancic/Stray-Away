using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : Interactable
{
    public Image CutsceneImage;
    public TextScroller TextScroller;
    public GameObject CutsceneToActivate;


    public override void DoOnInteract()
    {
        CutsceneImage.gameObject.SetActive(true);
        CutsceneImage.canvasRenderer.SetAlpha(0.01f);
        CutsceneImage.CrossFadeAlpha(1.0f, 2.0f, false);
        Invoke("ActivateScene", 2);

    }

    private void ActivateScene()
    {
        CutsceneToActivate.SetActive(true);
    }
}
