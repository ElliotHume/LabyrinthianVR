using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Sigtrap.VrTunnellingPro;

public class MovementProvider : LocomotionProvider
{
    public List<XRController> controllers;
    private CharacterController characterController;
    private GameObject head;
    public float speed = 3.0f;
    public float gravityMultiplier = 1f;
    public GameObject player;
    bool movedThisUpdate = false, canFly = false, defy = false;
    public TunnellingMobile vignette;
    Vector3 oldPos;
    public float MaxSpeed=6f;
    public float MaxFOV=0.7f;
    public AudioSource footsteps;
    float currentFOVIntensity = 0f;

    protected override void Awake()
    {
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
        if (vignette == null) vignette = head.GetComponent<TunnellingMobile>();
    }

    private void Start()
    {
        PositionController();

        // OUTDATED 
        OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.High; // it's the maximum foveation level
        OVRManager.useDynamicFixedFoveatedRendering = true;

        //Debug.Log("Cameras #: "+Camera.allCamerasCount+"   current: "+Camera.current.gameObject+"     main: "+Camera.main.gameObject);
        foreach(Camera item in Camera.allCameras) {
            Debug.Log("Camera: "+item.gameObject+"    isMain: "+(item == Camera.main)+ "    isCurrent: "+(item == Camera.current));
        }
        
    }

    private void Update()
    {
        PositionController();
        CheckForInput();
        ApplyGravity();
    }

    private void PositionController()
    {
        //Debug.Log("PRE: "+head.transform.position);
        // Get the head in local, playspace ground
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 0.2f,2f);
        characterController.height = headHeight;

        // Cut in half, add skin
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        // Let's move the capsule in local space as well
        newCenter.x = head.transform.localPosition.x;
        newCenter.z = head.transform.localPosition.z;

        // Apply
        characterController.center = newCenter;
    }

    private void CheckForInput()
    {
        movedThisUpdate = false;
        foreach(XRController controller in controllers) {
            if(controller.enableInputActions && !movedThisUpdate) {
                movedThisUpdate = CheckForMovement(controller.inputDevice);
            }
        }
        if (movedThisUpdate) {
            if (!footsteps.isPlaying) footsteps.Play();
        } else {
            footsteps.Stop();
        }
    }

    private bool CheckForMovement(InputDevice device)
    {
        if (canFly) {
            device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool goUp);
            device.TryGetFeatureValue(CommonUsages.primaryButton, out bool goDown);
            if (goDown){
                characterController.Move(Vector3.down * 4 * Time.deltaTime);
            }
            if (goUp){
                characterController.Move(Vector3.up * 4 * Time.deltaTime);
            }
        }

        // //  DEBUG CODE
        if (Input.GetKey("up")) {
            StartMove(new Vector3(0.5f, 0f, 0f));
        } else if (Input.GetKey("down")) {
            StartMove(new Vector3(-0.5f, 0f, 0f));
        } else if (Input.GetKey("right")) {
            StartMove(new Vector3(0f, 0f, 0.5f));
        } else if (Input.GetKey("left")) {
            StartMove(new Vector3(0f, 0f, -0.5f));
        }

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position)) {
            return StartMove(position);
            // if (StartMove(position)){
            //     return true;
            // } 
            // else if (vignette.forceVignetteValue > 0f){
            //     // Expand FOV
            //     vignette.forceVignetteValue = Mathf.Clamp(vignette.forceVignetteValue-0.01f, 0f, 0.7f); 
            //     print("Expanding");
            // }
        }

        return false;
    }

    private bool StartMove(Vector2 position)
    {
        // Apply the touch position to the head's forward Vector
        Vector3 direction = new Vector3(position.x, 0, position.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        // Rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;

        // Apply speed and move
        float speedModifier = canFly ? 3f : defy ? 1.5f : 1f;
        Vector3 movement = direction * (speed * speedModifier);
        Vector3 prevPos = head.transform.position;
        characterController.Move(movement * Time.deltaTime);

        // // Limit FOV when moving
        // if (movement.magnitude * Time.deltaTime > 0.01f) {
        //     vignette.forceVignetteValue = Mathf.Clamp(vignette.forceVignetteValue+0.05f, 0f, 0.7f);
        //     print("Shrinking:    "+movement.magnitude * Time.deltaTime);
        //     return true;
        // }

        // foreach(Camera c in Camera.allCameras) {
        //     print(c.gameObject + "    Position:   " + c.gameObject.transform.position);
        // }

        return (movement.magnitude * Time.deltaTime > 0.01f);
    }

    private void ApplyGravity() {
        if (!characterController.isGrounded && !canFly && !defy) {
            characterController.Move( new Vector3(0, Physics.gravity.y * gravityMultiplier  * Time.deltaTime, 0));
        }
    }

    public void ToggleFlight(bool val) {
        canFly = val;
    }

    public void ToggleDefyGravity(bool val) {
        defy = val;
    }

    public void LimitFOV(){
        Vector3 velocity = (transform.position - oldPos) / Time.deltaTime;
        oldPos = transform.position;

        float expectedLimit = MaxFOV;
        if (velocity.magnitude < MaxSpeed) {
            expectedLimit = (velocity.magnitude / MaxSpeed) * MaxFOV;
        }

        currentFOVIntensity = Mathf.Lerp(currentFOVIntensity, expectedLimit, 0.01f);

        vignette.forceVignetteValue = currentFOVIntensity;
    }
}