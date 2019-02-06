using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour {

    public Image CutsceneImage;
    public TextScroller TextScroller;
    public Sprite[] Sprites;

    private int _sceneNumber = 1;
    private bool _firstLoaded = false;


    void Start()
    {
        ActivateFirstScene();
    }

    private void Update()
    {
        if (_firstLoaded)
        {
            if (Input.anyKeyDown)
            {

                switch (_sceneNumber)
                {
                    case 1:
                        StartCoroutine(MultiFrameScene("But then, by Nature's forces, the World went to Shit."));
                        break;
                    case 2:
                        SingleScene("The apocalypse shrunk the population, and introduced the Bringers of Death who'd take care of the rest.", Sprites[4]);
                        break;
                    case 3:
                        SingleScene("Through decades your Race evolved the technology of the Old World, trying to get by in this desolate wasteland.", Sprites[5]);
                        break;
                    default:
                        break;
                }

                _sceneNumber++;

                if (_sceneNumber > 4)
                {
                    CutsceneImage.sprite = null;
                    CutsceneImage.gameObject.SetActive(false);
                    TextScroller.ScrollText("");
                    Destroy(gameObject);
                }
            }
        }
    }


    private IEnumerator MultiFrameScene(string text)
    {
        TextScroller.ScrollText(text);
        CutsceneImage.sprite = Sprites[1];
        yield return new WaitForSeconds(1.0f);
        CutsceneImage.sprite = Sprites[2];
        yield return new WaitForSeconds(1.0f);
        CutsceneImage.sprite = Sprites[3];




        yield return null;
    }



    private void SingleScene(string text, Sprite sprite)
    {
        CutsceneImage.sprite = sprite;
        TextScroller.ScrollText(text);
    }

    public void ActivateFirstScene()
    {
        SingleScene("Once upon a time there was humanity, on a home called Earth.", Sprites[0]);
        _firstLoaded = true;
    }
}
