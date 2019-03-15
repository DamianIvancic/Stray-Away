using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroller : MonoBehaviour
{
    public float delay = 0.05f;
    public GameObject DialogueBox;


    private Text _displayText;

    [HideInInspector]
    public bool TextFinished;
    private string _fullText;
    private string _textSegment;

    private bool _initialized = false;

    public void Start()
    {
        _displayText = GetComponent<Text>();

        ItemPickup.OnItemPickedUpCallback += PowerCellListener;
    }


    void Update()
    {
        if(TextFinished)
        {
            if (Input.anyKeyDown)
            {
                GameManager.GM.gameState = GameManager.GameState.Playing;
                _displayText.text = "";
                DialogueBox.SetActive(false); // turn off entire text box, not just text
            }
        }
    }

    IEnumerator ScrollText()
    {    
        TextFinished = false;

        for (int i = 0; i <= _fullText.Length; i++)
        {
            _textSegment = _fullText.Substring(0, i);
            _displayText.text = _textSegment;
            yield return new WaitForSeconds(delay);
        }

        TextFinished = true;
    }

    void PowerCellListener(Item item)
    {
        if (item.ItemName == "PowerCell" && InventoryManager.Instance.Inventory.GetCount(item) == 1)
            DisplayText("It's an object of highly concentrated energy. Perhaps it's from the meteor... But what is it doing here?");
    }

    public void DisplayText(string text)
    {
        GameManager.GM.gameState = GameManager.GameState.Dialogue;
        GameManager.GM.Player.ResetMovement();
        DialogueBox.SetActive(true);

        _fullText = text;
        StartCoroutine(ScrollText());
    }
}
