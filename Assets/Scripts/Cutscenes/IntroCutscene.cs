using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour {

    public Image CutsceneImage;
    public List<Sprite> Images;
  //  public List<SubScene> SubScenes;

    /*[System.Serializable]
    public class SubScene
    {
        public string DisplayText;
        public bool IsMultiFrameScene;
        public List<Sprite> Images;
    }*/

    #region TextScroller
    public float delay = 0.05f;
    [HideInInspector]
    public bool TextFinished;
    private string _fullText;
    private string _textSegment;
    private Text _text;
    #endregion

    private int _imageNumber = 1;
    private bool _firstLoaded = false;

    private IntroCutscene _thisScript;

    void Start()
    {
        _thisScript = GetComponent<IntroCutscene>();
        _text = gameObject.transform.parent.gameObject.GetComponentInChildren<Text>();

        ActivateFirstScene();  
    }

    private void Update()
    {

        if (_firstLoaded && TextFinished)
        {
            if (Input.anyKeyDown)
            {
                switch (_imageNumber)
                {
                    case 1:
                        StartCoroutine(MultiFrameScene("But then, by Nature's forces, the World went to Shit."));
                        break;
                    case 2:
                        SingleScene("The apocalypse shrunk the population, and introduced the Bringers of Death who'd take care of the rest.", Images[4]);
                        break;
                    case 3:
                        SingleScene("Through decades your Race evolved the technology of the Old World, trying to get by in this desolate wasteland.", Images[5]);
                        break;
                    default:
                        break;
                }

                _imageNumber++;

                if (_imageNumber > 4)
                {
                    CutsceneImage.sprite = null;
                    CutsceneImage.gameObject.SetActive(false);
                    _text.text = "";
                    _thisScript.enabled = false;
                }
            }
        }
    }


    private IEnumerator MultiFrameScene(string text)
    {
        DisplayText(text);
        CutsceneImage.sprite = Images[1];
        yield return new WaitForSeconds(1.0f);
        CutsceneImage.sprite = Images[2];
        yield return new WaitForSeconds(1.0f);
        CutsceneImage.sprite = Images[3];

        yield return null;
    }


    private void SingleScene(string text, Sprite sprite)
    {
        CutsceneImage.sprite = sprite;
        DisplayText(text);
    }

    public void ActivateFirstScene()
    {
         SingleScene("Once upon a time there was humanity, on a home called Earth.", Images[0]);
        _firstLoaded = true;
    }

    public void DisplayText(string text)
    {
        _fullText = text;
        StartCoroutine(ScrollText());
    }

    IEnumerator ScrollText()
    {
        TextFinished = false;

        for (int i = 0; i <= _fullText.Length; i++)
        {
            _textSegment = _fullText.Substring(0, i);
            _text.text = _textSegment;
            yield return new WaitForSeconds(delay);
        }

        TextFinished = true;
    }
}
