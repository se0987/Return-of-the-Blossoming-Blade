using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public int playMusicTrack;
    private BGMManager bgmManager;

    public bool enable = false;

    // Start is called before the first frame update
    void Start()
    {
        bgmManager = FindObjectOfType<BGMManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enable)
        {
            bgmManager.Play(playMusicTrack);
            enable = false;
        }
    }

}
