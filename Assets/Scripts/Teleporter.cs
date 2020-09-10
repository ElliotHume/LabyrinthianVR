﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public List<GameObject> triggerObjects;
    public GameObject player;
    public GameObject teleportAnchor;
    public bool sceneChanger;
    public string sceneName;

    public bool playerTriggered;

    public List<GameObject> toggleObjects;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = GameObject.Find("XR Rig");
        if (teleportAnchor == null) teleportAnchor = GameObject.Find("SpawnPoint");
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
            try {
                SceneManager.LoadScene(sceneName);
            } catch (System.Exception e) {
                Debug.Log("Error loading scene: "+e);
            }
            
        } else {
            player.transform.position = teleportAnchor.transform.position;
            player.transform.rotation = teleportAnchor.transform.rotation;

            if (toggleObjects.Count > 0 ) {
                foreach( GameObject go in toggleObjects ) {
                    go.SetActive(!go.activeInHierarchy);
                }
            }

            AudioSource asrce = GetComponent<AudioSource>();
            if (asrce != null) asrce.Play();
        }
    }
}
