using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Binding : MonoBehaviour
{
    // Player fields
    public Transform anchor;
    XRController handController;
    GameObject castingHand;

    // Bound object fields
    bool hasBoundObject = false;
    GameObject boundObject;
    Transform oldParent;
    Vector3 objectGrabOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (boundObject != null) boundObject.transform.position = anchor.position; //+ objectGrabOffset;

        if (handController != null) {
            handController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool goAway);
            handController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool comeTowards);
            handController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float letGo);

            if (letGo > 0.8f || Input.GetKeyDown("p")) {
                boundObject.GetComponent<Rigidbody>().useGravity = true;
                boundObject = null;
                hasBoundObject = false;

                Destroy(gameObject, 0.1f);
                return;
            } else if (comeTowards || Input.GetKey("k")) {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - 0.025f);
            } else if (goAway || Input.GetKey("j")) {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + 0.025f);
            }
        }
        
    }

    public void LinkCastingHand(GameObject hand) {
        castingHand = hand;
        handController = hand.GetComponent<XRController>();
    }

    void OnTriggerEnter(Collider other) {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null && !hasBoundObject) {
            boundObject = other.gameObject;
            objectGrabOffset = anchor.position - other.ClosestPoint(anchor.position);
            hasBoundObject = true;
        }
    }
}
