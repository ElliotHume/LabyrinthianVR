using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftHandManager : MonoBehaviour
{
    public XRController controller;
    private InputDevice inputDevice;
    public XRDirectInteractor interactor;
    public List<InputHelpers.Button> startMoveButtons;
    public InputHelpers.Button grip;  
    public float activationThreshold = 0.2f;

    public Player player;
    public bool held = false;

    public GameObject drawingSphere;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
        inputDevice = controller.inputDevice;
        interactor = GetComponent<XRDirectInteractor>();
    }

    public bool CheckIfActivated(XRController controller){
        InputHelpers.IsPressed(controller.inputDevice, grip, out bool isGripped, activationThreshold);
        return (isGripped);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            try {
                player = GameObject.Find("XR Rig").GetComponent<Player>();
            } catch {
                // do nothing
            }

        if (drawingSphere != null && CheckIfActivated(controller) && !held){
            held = true;
            drawingSphere.SetActive(true);
        }

        if (drawingSphere != null && !CheckIfActivated(controller) && held) {
            held = false;
            drawingSphere.SetActive(false);
        }
    }
}
