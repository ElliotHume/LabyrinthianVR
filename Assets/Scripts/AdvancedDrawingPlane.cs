﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using AdVd.GlyphRecognition;
using UnityEngine.SceneManagement;

public class AdvancedDrawingPlane : MonoBehaviour
{
    public string handName;
    public GameObject hand, playerGO;
    public CharacterController playerController;
    public GlyphDrawInput glyphDrawInput;
    public GlyphRecognition glyphRecognition;
    public Player player;
    public ParticleSystem edgeParticles;
    public GameObject spellDisplayPanel;

    public XRController controller;
    public InputHelpers.Button drawButton, showSpellsButton; 
    public float activationThreshold = 0.1f, planeOffsetForward = 0.15f;

    ContactPoint lastContactPoint;
    MeshRenderer meshRenderer;
    XRDirectInteractor interactor;
    BoxCollider boxCollider;
    bool visible = false, forceVisible = false;


    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        if (player == null) player = GameObject.Find("XR Rig").GetComponent<Player>();
        if (playerGO == null ) playerGO = GameObject.Find("XR Rig");
        if (playerController == null) playerController = GameObject.Find("XR Rig").GetComponent<CharacterController>();
        meshRenderer = GetComponent<MeshRenderer>();
        interactor = hand.GetComponent<XRDirectInteractor>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get player controller position in worldspace
        Vector3 playerPos = playerGO.transform.TransformPoint(playerController.center + new Vector3(0f, playerController.height / 2f, 0));

        if (!CheckIfActivated()) {
            transform.LookAt((hand.transform.position + (playerPos)) /2f);
            // transform.LookAt(playerPos);
            transform.position = hand.transform.position + (planeOffsetForward * hand.transform.forward);
            if (visible && !forceVisible) {
                boxCollider.enabled = false;
                meshRenderer.enabled = false;  
                visible = false;
                if (edgeParticles) edgeParticles.Stop();
                if (spellDisplayPanel != null) spellDisplayPanel.SetActive(false);
            }
        } else {
            if (!visible && !interactor.isSelectActive) {
                boxCollider.enabled = true;
                meshRenderer.enabled = true;
                visible = true;
                if (edgeParticles) edgeParticles.Play();
            }
        }

        InputHelpers.IsPressed(controller.inputDevice, showSpellsButton, out bool showSpells, activationThreshold);
        string heldSpell = (handName == "right") ? player.rightHandSpell : player.leftHandSpell;
        if ((showSpells || Input.GetKey("v") && (heldSpell == null || heldSpell == ""))) {
            if (spellDisplayPanel != null) spellDisplayPanel.SetActive(true);
        } else {
            if (spellDisplayPanel != null) spellDisplayPanel.SetActive(false);
        }

        if (Input.GetKeyDown("4")) {
            planeOffsetForward += 0.5f;
            forceVisible = true;
            boxCollider.enabled = true;
            meshRenderer.enabled = true;
            visible = true;
            if (edgeParticles) edgeParticles.Play();
        } else if (Input.GetKeyDown("5")) {
            planeOffsetForward -= 0.1f;
        }
    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log("ENTER: "+collision.gameObject);
        if (collision.gameObject.tag == "Player") {
            ContactPoint contact = collision.contacts[0];
            Vector3 localSpacePoint = transform.InverseTransformPoint(contact.point);
            Vector2 twospace = new Vector2(-localSpacePoint.x, localSpacePoint.y);
            try {
                glyphDrawInput.BeginCustomDrag(twospace);
            } catch {
                print("failure to start drag");
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        //Debug.Log("STAY:  "+collision.gameObject);
        if (collision.gameObject.tag == "Player") {
            ContactPoint contact = collision.contacts[0];
            lastContactPoint = contact;
            Vector3 localSpacePoint = transform.InverseTransformPoint(contact.point);
            Vector2 twospace = new Vector2(-localSpacePoint.x, localSpacePoint.y);
            try {
                glyphDrawInput.CustomDrag(twospace);
            } catch {
                print("failure to drag");
            }
        }  
    }

    void OnCollisionExit(Collision collision) {
        //Debug.Log("END: " +collision.gameObject);
        if (collision.gameObject.tag == "Player") {
            ContactPoint contact = lastContactPoint;
            Vector3 localSpacePoint = transform.InverseTransformPoint(contact.point);
            Vector2 twospace = new Vector2(-localSpacePoint.x, localSpacePoint.y);
            try {
                glyphDrawInput.EndCustomDrag(twospace);
            } catch {
                print("failure to end drag");
            }
        }
    }

    public bool CheckIfActivated(){
        InputHelpers.IsPressed(controller.inputDevice, drawButton, out bool isGripped, activationThreshold);
        string heldSpell = (handName == "right") ? player.rightHandSpell : player.leftHandSpell;
        return (isGripped && (heldSpell == null || heldSpell == ""));
    }


    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
