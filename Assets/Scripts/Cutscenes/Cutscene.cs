using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public string Name;
    public Image CutsceneImage;

    [System.Serializable]
    public class SubScene
    {
        public string DisplayText;      
        public List<Sprite> Images;
    }

    public List<SubScene> SubScenes;

    private int _subSceneIdx = 0;
    private bool _isEndingCutscene = false;

    #region TextScroller
    public Text Text;
    public float delay = 0.05f;
    [HideInInspector]
    public bool TextFinished;
    private string _fullText;
    private string _textSegment;
    #endregion

    public delegate void OnCutsceneFinished(string cutsceneName);
    public static OnCutsceneFinished OnCutsceneFinishedCallback;
   
    void Start()
    {     
        if (Name == "EscapeCutscene" || Name == "DefendCutscene")
            _isEndingCutscene = true;     
    }

    void Update()
    {
        if(TextFinished  && Input.anyKeyDown)
        {
            if (_subSceneIdx == SubScenes.Count)
            {
                OnCutsceneFinishedCallback.Invoke(Name);
                           
                if(!_isEndingCutscene)
                  gameObject.SetActive(false);
            }
            else
                StartCoroutine(PlaySubScene(SubScenes[_subSceneIdx]));      
        }            
    }

    IEnumerator PlaySubScene(SubScene subScene)
    {
         DisplayText(subScene.DisplayText);

         foreach(Sprite image in subScene.Images)
         {
            CutsceneImage.sprite = image;
            yield return new WaitForSeconds(1.0f);
         }

        _subSceneIdx++;

        yield return null;
    }

    void DisplayText(string text)
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
            Text.text = _textSegment;
            yield return new WaitForSeconds(delay);
        }

        TextFinished = true;
    }

    public void Play()
    {     
        gameObject.SetActive(true);
        _subSceneIdx = 0;
        StartCoroutine(PlaySubScene(SubScenes[_subSceneIdx]));
    }

}
