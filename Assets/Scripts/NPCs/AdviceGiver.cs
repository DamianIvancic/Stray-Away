using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdviceGiver : Interactable
{
    private List<string> _dialogue;
    private TextScroller _dialogueText;
    private int _dialogueIdx;

    void Start()
    {
        _dialogueText = GameManager.GM.UI.DialogueText;
        InitializeDialogue();
    }

    void InitializeDialogue()
    {
        _dialogue = new List<string>();
        _dialogue.Add("A word of advice before you head out - when you're not sure what your options are in a given situation, just hit escape and go to options -> controls to remind yourself what you're capable of.");
        _dialogue.Add("Be careful out there in the woods. I almost got lost the other day. Luckily I got a good sense of my surroundings by zooming out with the mouse scrollwheel and found my way back.");
        _dialogue.Add("If you ever feel too tired to go on just come back home and you'll feel better before long.");
    }

    public override void DoOnInteract()
    {
        _dialogueText.DisplayText(_dialogue[_dialogueIdx]);

        if (_dialogue.Count > 1)
        {
            _dialogueIdx++;

            if (_dialogueIdx == _dialogue.Count)
                _dialogueIdx = 0;
        }
    }
}
