using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        LoadingManager.LoadScene(sceneName);
    }
}
