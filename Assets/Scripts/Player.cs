using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{

    private Vector3 startGripMove, releaseGripMove;
    public int dragMoveSpeed = 10;
    private float movementCooldown = 0f;
    public GameObject castingHand, movingHand, spellReticle, baseReticle;
    public XRController castingHandController, movingHandController;
    public XRRayInteractor castingRay;
    public XRInteractorLineVisual castingLineRenderer;
    public SkinnedMeshRenderer castingHandRenderer, movementHandRenderer;
    public Material baseMaterial, spellHandGlow, movementCooldownGlow;
    public ParticleSystem fireParticles, lightningParticles, windParticles, royalParticles, iceParticles, damageParticles;
    public GameObject shieldSphere, arcanoSphere;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ControllerJoystickMove (Vector2 position)
    {
        // Apply the touch position to the head's forward Vector
        Vector3 direction = new Vector3(position.x, 0, position.y);
        Vector3 headRotation = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

        // Rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;

        // Apply speed and move
        Vector3 movement = direction * 3;
        transform.position += movement * Time.deltaTime;
    }

    public void StartGripMove(Vector3 startPos){
        startGripMove = new Vector3(startPos.x, 0f, startPos.z);
        //Debug.Log("StartGripMove:  "+startGripMove.ToString());
        movingHandController.inputDevice.SendHapticImpulse(0, 0.5f, 0.3f);
    }

    public void ReleaseGripMove(Vector3 endPos) {
        releaseGripMove = new Vector3(endPos.x, 0f, endPos.z);

        if (movementCooldown == 0) {
            Vector3 movement = startGripMove - releaseGripMove;
            //Debug.Log("Movement: "+movement.ToString()+ "   Magnitude: "+movement.magnitude);

            // Move player in direction of pull * speed
            transform.position += movement * dragMoveSpeed;

            // Set hand cooldown material
            //movementHandRenderer.material = movementCooldownGlow;

            // Set movement Cooldown
            //movementCooldown = 2.5f;

            
            try {
                movingHandController.inputDevice.SendHapticImpulse(0, 2f, 0.1f);
                //movingHand.GetComponent<AudioSource>().Play();
            } catch {
                Debug.Log("Failure to send haptics or play sound effect");
            }
            
        } else {
            Debug.Log("Movement on cooldown");
        }
    }
}
