using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] int sceneNum = 1;
    void OnMouseDown()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneNum);
    }
}
