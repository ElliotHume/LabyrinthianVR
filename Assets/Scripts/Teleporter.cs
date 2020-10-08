using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Teleporter : MonoBehaviour
{
    public List<GameObject> triggerObjects;
    public GameObject player;
    public GameObject teleportAnchor;
    public bool sceneChanger;
    public string sceneName;
    public bool playerTriggered;
    public UnityEvent onTeleport;

    PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = GameObject.Find("XR Rig");
        if (teleportAnchor == null) teleportAnchor = GameObject.Find("SpawnPoint");
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if ( triggerObjects.Contains(other.gameObject) || (playerTriggered && other.tag == "Player") ) {
            Debug.Log(gameObject.name + " teleporter hit");
            Teleport();
        }
    }

    void Teleport() {
        if ( sceneChanger ) {
            playerManager.LoadScene(sceneName);
        } else {
            player.transform.position = teleportAnchor.transform.position;
            player.transform.rotation = teleportAnchor.transform.rotation;

            onTeleport.Invoke();

            AudioSource asrce = GetComponent<AudioSource>();
            if (asrce != null) asrce.Play();
        }
    }
}
