using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandManager : MonoBehaviour
{
    public XRController controller;
    private InputDevice inputDevice;
    // private XRRayInteractor rayInteractor;
    // private XRInteractorLineVisual lineVisual;

    public bool held = false;
    public InputHelpers.Button grip; 
    public GlyphRecognition glyphRecognition;
    public float activationThreshold = 0.1f;
    public Player player;

    public GameObject drawingSphere;

    public GameObject castingLine;
    public XRDirectInteractor interactor;
    //private float lineWidth = 0.1f; 

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
        inputDevice = controller.inputDevice;
        interactor = GetComponent<XRDirectInteractor>();
        // rayInteractor = GetComponent<XRRayInteractor>();
        // lineVisual = GetComponent<XRInteractorLineVisual>();
        // lineVisual.enabled = false;
        //lineVisual.reticle.SetActive(false);
    }

    public bool CheckIfActivated(XRController controller){
        InputHelpers.IsPressed(controller.inputDevice, grip, out bool isGripped, activationThreshold);
        return (isGripped);
    }

    // public bool CheckIfRayHit(XRController controller){
    //     return rayInteractor.GetCurrentRaycastHit(out RaycastHit rayhit);
    // }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            try {
                player = GameObject.Find("XR Rig").GetComponent<Player>();
            } catch {
                // do nothing
            }
			

        // if (rayInteractor && !held && lineVisual.enabled) {
        //     lineVisual.enabled = false;
        //     lineVisual.reticle.SetActive(false);
        // }   

        //if (CheckIfActivated(controller)) { glyphRecognition.Cast(); }

        if (player != null && CheckIfActivated(controller) && !held){
            //print("try cast");
            held = true;
            drawingSphere.SetActive(false);
            if (interactor.selectTarget == null) {
                castingLine.SetActive(true);
                glyphRecognition.Cast();
            }
        }

        if (player != null && !CheckIfActivated(controller) && held) {
            held = false;
            drawingSphere.SetActive(true);
            castingLine.SetActive(false);
            if (interactor.selectTarget == null) {
                player.ReleaseSpellCast();
            }
        }

        //controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed);
        //if (pressed) player.CastFireball(0, 0f);
    }
}
