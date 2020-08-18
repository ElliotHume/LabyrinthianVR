using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandManager : MonoBehaviour
{
    public XRController controller;
    private InputDevice inputDevice;
    private XRRayInteractor rayInteractor;
    private XRInteractorLineVisual lineVisual;

    public bool held = false;
    public InputHelpers.Button grip; 
    public GlyphRecognition glyphRecognition;
    public float activationThreshold = 0.1f;
    public Player player;

    public GameObject drawingSphere;

    public LineRenderer lineRenderer;
    //private float lineWidth = 0.1f; 

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
        inputDevice = controller.inputDevice;
        rayInteractor = GetComponent<XRRayInteractor>();
        lineVisual = GetComponent<XRInteractorLineVisual>();
        lineVisual.enabled = false;


        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        Vector3[] initLaserPositions = new Vector3[ 2 ] { Vector3.zero, Vector3.zero };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions( initLaserPositions );
        //lineRenderer.SetWidth(lineWidth, lineWidth);
        //lineVisual.reticle.SetActive(false);
    }

    public bool CheckIfActivated(XRController controller){
        InputHelpers.IsPressed(controller.inputDevice, grip, out bool isGripped, activationThreshold);
        return (isGripped);
    }

    public bool CheckIfRayHit(XRController controller){
        return rayInteractor.GetCurrentRaycastHit(out RaycastHit rayhit);
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
			

        // if (rayInteractor && !held && lineVisual.enabled) {
        //     lineVisual.enabled = false;
        //     lineVisual.reticle.SetActive(false);
        // }   

        //if (CheckIfActivated(controller)) { glyphRecognition.Cast(); }

        if (player != null && CheckIfActivated(controller) && !held){
            //print("try cast");
            held = true;
            drawingSphere.SetActive(false);
            glyphRecognition.Cast();
            ShowRay(50f);
            lineRenderer.enabled = true;
        }

        if (player != null && !CheckIfActivated(controller) && held) {
            held = false;
            drawingSphere.SetActive(true);
            lineRenderer.enabled = false;
            player.ReleaseSpellCast();
            
        }

        //controller.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed);
        //if (pressed) player.CastFireball(0, 0f);
    }

    void ShowRay( float length ) {
         Ray ray = new Ray( transform.position, transform.eulerAngles );
         RaycastHit raycastHit;
         Vector3 endPosition = transform.position + ( length * transform.eulerAngles );
 
         if( Physics.Raycast( ray, out raycastHit, length ) ) {
             endPosition = raycastHit.point;
         }
 
         print("here");
         lineRenderer.SetPosition( 0, transform.position );
         lineRenderer.SetPosition( 1, endPosition );
     }
}
