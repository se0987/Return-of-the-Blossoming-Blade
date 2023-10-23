using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public string SceneName;
    //public BTNType currentType;
    public void OnBtnClick()
    {
        Debug.Log(SceneName);
        SceneManager.LoadScene(SceneName);
    }
}
