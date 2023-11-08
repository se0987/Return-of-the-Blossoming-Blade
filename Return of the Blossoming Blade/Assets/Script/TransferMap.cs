using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;

    public Transform target;
    public BoxCollider2D targetBound;
    public string gateName;

    private PlayerManager thePlayer;
    private CameraManager theCamera;

    private AudioManager theAudio;

    public bool move = false;
    public string moveSound;

    public bool stop;
    private BGMManager bgmManager;

    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theAudio = FindObjectOfType<AudioManager>();
        bgmManager = FindObjectOfType<BGMManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && move)
        {
            theAudio.Play(moveSound);
            if (stop)
            {
                bgmManager.Stop();
            }
            thePlayer.currentMapName = transferMapName;
            theCamera.SetBound(targetBound);
            theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
            thePlayer.transform.position = target.transform.position;
        }
    }
}
