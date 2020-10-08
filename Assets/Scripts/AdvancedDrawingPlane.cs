using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using AdVd.GlyphRecognition;
using UnityEngine.SceneManagement;

public class AdvancedDrawingPlane : MonoBehaviour
{
    public string handName;
    public GameObject hand;
    public Camera playerCamera;
    public GlyphDrawInput glyphDrawInput;
    public GlyphRecognition glyphRecognition;
    public Player player;
    public ParticleSystem edgeParticles;

    public XRController controller;
    public InputHelpers.Button drawButton; 
    public float activationThreshold = 0.1f, planeOffsetForward = 0.15f;

    ContactPoint lastContactPoint;
    MeshRenderer meshRenderer;
    XRDirectInteractor interactor;
    BoxCollider boxCollider;
    bool visible = false;


    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        if (player == null) player = GameObject.Find("XR Rig").GetComponent<Player>();
        meshRenderer = GetComponent<MeshRenderer>();
        interactor = hand.GetComponent<XRDirectInteractor>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCamera) playerCamera = Camera.main;

        if (!CheckIfActivated()) {
            transform.LookAt((hand.transform.position + playerCamera.transform.position) / 2);
            transform.position = hand.transform.position + (planeOffsetForward * hand.transform.forward);
            if (visible) {
                boxCollider.enabled = false;
                meshRenderer.enabled = false;  
                visible = false;
                if (edgeParticles) edgeParticles.Stop();
            }
        } else {
            if (!visible && !interactor.isSelectActive) {
                boxCollider.enabled = true;
                meshRenderer.enabled = true;
                visible = true;
                if (edgeParticles) edgeParticles.Play();
            }
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
