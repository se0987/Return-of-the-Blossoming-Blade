using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager instance;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public TextMeshProUGUI text;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
    }

    public void ShowChapter(string _chapter)
    {
        text.text = _chapter;
        StartCoroutine(StartChapterCoroutine());
    }

    IEnumerator StartChapterCoroutine()
    {
        anim.SetBool("Appear", true);
        yield return new WaitForSeconds(130f);
        anim.SetBool("Appear", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
