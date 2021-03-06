﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using AdVd.GlyphRecognition;
using UnityEngine.SceneManagement;

public class DrawingPlane : MonoBehaviour
{
    public bool isPhysical = false;
    public Camera playerCamera;
    public GameObject blockingPlane;
    public GlyphDrawInput glyphDrawInput;
    public GlyphRecognition glyphRecognition;

    public XRController controller;
    public InputHelpers.Button enableRayButton; 
    public float activationThreshold = 0.1f;

    ContactPoint lastContactPoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCamera) playerCamera = Camera.main;

        transform.LookAt(playerCamera.transform);
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


    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
