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
        //이전값 삭제
        PlayerPrefs.DeleteKey("playerMapName");
        PlayerPrefs.DeleteKey("CJEvent2One");
        PlayerPrefs.DeleteKey("choice1");
        PlayerPrefs.DeleteKey("choice2");
        PlayerPrefs.DeleteKey("choice3");
        PlayerPrefs.DeleteKey("JeokCheonPlayTime");
        PlayerPrefs.DeleteKey("GwanghonPlayTime");
        PlayerPrefs.DeleteKey("ChunsalPlayTime");
    }
}
