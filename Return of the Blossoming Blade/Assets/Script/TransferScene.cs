using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferScene : MonoBehaviour
{
    public string transferMapName;

    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private DialogueManager theDialogue;

    public bool move = false;
    public string gateName;

    private AudioManager theAudio;

    public string doorSound;

    public bool stop;
    private BGMManager bgmManager;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theAudio = FindObjectOfType<AudioManager>();
        bgmManager = FindObjectOfType<BGMManager>();
        theDialogue = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && move)
        {
            StartCoroutine(LoadingCoroutine());
        }
    }

    IEnumerator LoadingCoroutine()
    {
        theDialogue.ShowLoading();
        yield return new WaitForSeconds(2f);
        thePlayer.currentMapName = transferMapName;
        SceneManager.LoadScene(transferMapName);
        if (stop)
        {
            bgmManager.Stop();
        }
        theAudio.Play(doorSound);
    }
}
