using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScroller : MonoBehaviour
{
    public float delay = 0.05f;
    private string FullText;
    private string _currentText;
    private bool _finished;


    void Update()
    {
        if(_finished)
        {
            if (Input.anyKeyDown)
                gameObject.transform.parent.gameObject.SetActive(false); // turn off entire text box, not just text
        }
    }



    public void ScrollText(string fullText)
    {
        FullText = fullText;
        StartCoroutine(ShowText());
    }


    IEnumerator ShowText()
    {
        _finished = false;

        for (int i = 0; i <= FullText.Length; i++)
        {
            _currentText = FullText.Substring(0, i);
            this.GetComponent<Text>().text = _currentText;
            yield return new WaitForSeconds(delay);
        }

        _finished = true;
    }
}
