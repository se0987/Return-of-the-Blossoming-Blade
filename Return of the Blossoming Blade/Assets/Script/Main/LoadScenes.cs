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
        SceneManager.LoadScene(SceneName);
    }
}
