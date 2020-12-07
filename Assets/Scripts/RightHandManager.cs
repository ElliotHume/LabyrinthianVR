using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandManager : MonoBehaviour
{
    public string handName;
    public XRController controller;
    private InputDevice inputDevice;
    // private XRRayInteractor rayInteractor;
    // private XRInteractorLineVisual lineVisual;

    public bool held = false;
    public InputHelpers.Button grip, trigger; 
    public GlyphRecognition glyphRecognition;
    public float activationThreshold = 0.1f;
    public Player player;

    public GameObject drawingSphere;

    public GameObject castingLine;
    public XRDirectInteractor interactor;
    //private float lineWidth = 0.1f; 

    bool castSuccess = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
        inputDevice = controller.inputDevice;
        if (interactor == null) interactor = GetComponent<XRDirectInteractor>();
    }

    public bool CheckIfActivated(XRController controller){
        InputHelpers.IsPressed(controller.inputDevice, grip, out bool isGripped, activationThreshold);
        return (isGripped);
    }

    public bool CheckIfTriggered(XRController controller){
        InputHelpers.IsPressed(controller.inputDevice, trigger, out bool isTriggered, activationThreshold);
        return (isTriggered);
    }


    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            try {
                player = GameObject.Find("XR Rig").GetComponent<Player>();
            } catch {
                // do nothing
            }
        } else {
            if (player != null && CheckIfActivated(controller) && !held) {
                //print("try cast");
                held = true;
                if (interactor.selectTarget == null) {
                    drawingSphere.SetActive(true);
                    if (castSuccess) {
                        castingLine.SetActive(true);
                    }
                }
                
            }

            if (player != null && !CheckIfActivated(controller) && held) {
                held = false;
                drawingSphere.SetActive(false);
                castingLine.SetActive(false);
                if (castSuccess) {
                    player.ReleaseSpellCast(handName);
                    castSuccess = false;
                }
                castSuccess = glyphRecognition.Cast();
            }

            if (player != null && CheckIfTriggered(controller) && held) {
                held = false;
                drawingSphere.SetActive(false);
                castingLine.SetActive(false);
                player.NullifySpellCast(handName);
                castSuccess = false;
            }
        }
        

        //controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed);
        //if (pressed) player.CastFireball(0, 0f);
    }
}
