using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource source;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(int _playMusicTrack, float volume = 0.5f)
    {
        source.volume = volume;
        source.clip = clips[_playMusicTrack];
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for(float i = 1.0f; i>=0f; i-= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0f; i <= 1.0f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }
}
