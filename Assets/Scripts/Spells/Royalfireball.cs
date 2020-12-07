using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Royalfireball : MonoBehaviour
{

    public GameObject royalFire;
    public XRGrabInteractable grabInteractable;

    void Start() {
        if (grabInteractable == null) grabInteractable = GetComponent<XRGrabInteractable>();
    }
    
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag != "Player") {
            if (collision.gameObject.tag != "NullZone") Instantiate(royalFire, transform.position, Quaternion.identity);
            if (grabInteractable != null) grabInteractable.colliders.Clear();
            Destroy(gameObject);
        }
    }

}
