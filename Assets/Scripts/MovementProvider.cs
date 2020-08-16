using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementProvider : LocomotionProvider
{
    public List<XRController> controllers;
    private CharacterController characterController;
    private GameObject head;
    public GameObject drawPanel;

    public GlyphRecognition glyphRecognition;

    public float speed = 3.0f;
    public float gravityMultiplier = 1.0f;

    public GameObject player;
    public GameObject drawingAnchor;

    private bool isHeadSetOnLastFrame;

    protected override void Awake()
    {
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
    }

    private void Start()
    {
        PositionController();
        // OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.High; // it's the maximum foveation level
        // OVRManager.useDynamicFixedFoveatedRendering = true;

        Debug.Log("Cameras #: "+Camera.allCamerasCount+"   current: "+Camera.current.gameObject+"     main: "+Camera.main.gameObject);
        foreach(Camera item in Camera.allCameras) {
            Debug.Log("Camera: "+item.gameObject+"    isMain: "+(item == Camera.main)+ "    isCurrent: "+(item == Camera.current));
        }
        
    }

    private void Update()
    {
        PositionController();
        CheckForInput();
        ApplyGravity();

        //if (characterController.detectCollisions) characterController.isTrigger = true;

        // if ( Vector3.Distance(drawingAnchor.transform.position, transform.position) > 0.5f ) {
        //     drawingAnchor.transform.position = Vector3.MoveTowards(drawingAnchor.transform.position, transform.position, 0.1f);
        // } 

    }
    private void PositionController()
    {
        //Debug.Log("PRE: "+head.transform.position);
        // Get the head in local, playspace ground
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1,2);
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
        //Debug.Log("POST: "+head.transform.position);
    }

    private void CheckForInput()
    {
        foreach(XRController controller in controllers) {
            if(controller.enableInputActions) {
                CheckForMovement(controller.inputDevice);
            }
        }
    }

    private void CheckForMovement(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position)) {
            StartMove(position);
        }


        // device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool goDown);
        // device.TryGetFeatureValue(CommonUsages.primaryButton, out bool goUp);
        // if (goDown){
        //     characterController.Move(Vector3.down * Time.deltaTime);
        // }
        // if (goUp){
        //     characterController.Move(Vector3.up * Time.deltaTime);
        // }
    }

    private void StartMove(Vector2 position)
    {
        //print("Position: "+head.transform.position+"      Rotation: "+head.transform.eulerAngles);
        // Apply the touch position to the head's forward Vector
        Vector3 direction = new Vector3(position.x, 0, position.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        // Rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;

        // Apply speed and move
        Vector3 movement = direction * speed;
        Vector3 prevPos = head.transform.position;
        characterController.Move(movement * Time.deltaTime);
        //drawingAnchor.transform.position += movement * Time.deltaTime;



        // if (movement.magnitude > 0f) {
        //     Debug.Log("Movement, playerPos:  "+player.transform.position+"    characterControllerPos: "+characterController.transform.position+"   headPos: "+head.transform.position);
        //     foreach(Camera item in Camera.allCameras) {
        //         Debug.Log("Camera: "+item.gameObject+"  position: "+item.gameObject.transform.position+"             isMain: "+(item == Camera.main)+ "    isCurrent: "+(item == Camera.current));
        //     }
        // }
    }

    private void ApplyGravity() {
        if (!characterController.isGrounded) {
            Vector3 gravity = new Vector3(0, Physics.gravity.y * gravityMultiplier, 0);
            gravity.y *= Time.deltaTime;

            characterController.Move(gravity * Time.deltaTime);
        }
    }
}