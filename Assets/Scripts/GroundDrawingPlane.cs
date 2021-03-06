﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using AdVd.GlyphRecognition;
using UnityEngine.SceneManagement;

public class GroundDrawingPlane : MonoBehaviour
{
    public GlyphDrawInput glyphDrawInput;
    public GlyphRecognition glyphRecognition;

    // Spell prefabs
    public GameObject ascend, protection, hearthbind, domain; 

    ContactPoint lastContactPoint;
    BoxCollider boxCollider;
    bool visible = false, forceVisible = false;
    float castAccuracy = 0f;


    void Awake()
    {
        if (glyphRecognition == null) glyphRecognition = GetComponent<GlyphRecognition>();

        if (glyphRecognition && !glyphRecognition.groundCast) {
            glyphRecognition.ActivateGroundDraw(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        //Debug.Log("ENTER: "+collision.gameObject);
        if (collision.gameObject.tag == "Player") {
            ContactPoint contact = collision.contacts[0];
            Vector3 localSpacePoint = transform.InverseTransformPoint(contact.point);
            Vector2 twospace = new Vector2(localSpacePoint.x, localSpacePoint.z);
            try {
                glyphDrawInput.BeginCustomDrag(twospace);
            } catch {
                print("failure to start drag");
            }
        } else if (collision.gameObject.tag == "SpellComponent") {
            ProcessSpellComponent(collision.gameObject);
        }
    }

    void OnCollisionStay(Collision collision) {
        //Debug.Log("STAY:  "+collision.gameObject);
        if (collision.gameObject.tag == "Player") {
            ContactPoint contact = collision.contacts[0];
            lastContactPoint = contact;
            Vector3 localSpacePoint = transform.InverseTransformPoint(contact.point);
            Vector2 twospace = new Vector2(localSpacePoint.x, localSpacePoint.z);
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
            Vector2 twospace = new Vector2(localSpacePoint.x, localSpacePoint.z);
            try {
                glyphDrawInput.EndCustomDrag(twospace);
            } catch {
                print("failure to end drag");
            }
        }
    }

    public void ProcessSpellComponent(GameObject g) {
        // Debug.Log("Processing component");
        SpellComponent scomponent = g.GetComponent<SpellComponent>();

        // if you cant grab a spellcomponent from the object, stop the function
        if (scomponent == null) return;

        switch (scomponent.component.name) {
            case "activate":
                Debug.Log("Activate ground circle");
                glyphRecognition.Cast();
                scomponent.Break();
                break;
        }
    }

    public void CastSpell(string spell, float accuracy) {
        // TODO: actually do something with this
        // Round accuracy results for more consistent spell scaling effects
        if (accuracy >= 0.74f) {
            accuracy = 1f;
        } else if (accuracy >= 0.64f) {
            accuracy = 0.75f;
        } else if (accuracy >= 0.44f ) {
            accuracy = 0.5f;
        }
        castAccuracy = accuracy;

        GameObject spawnPrefab = null;
        float verticalOffset = 0f;
        switch (spell) {
            case "ascend":
                spawnPrefab = ascend;
                verticalOffset = 1.8f;
                break;
            case "protection":
                spawnPrefab = protection;
                break;
            case "hearthbind":
                spawnPrefab = hearthbind;
                break;
            case "domain":
                spawnPrefab = domain;
                break;
        }

        Instantiate(spawnPrefab, transform.position+(transform.up * verticalOffset), transform.rotation);
        glyphDrawInput.enabled = false;
        Destroy(gameObject, 30f);
    }
}
