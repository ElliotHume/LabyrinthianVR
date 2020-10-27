using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HandLinkPuzzle : MonoBehaviour
{
    bool active = false;
    public GameObject leftHand, rightHand;

    public Rigidbody l_object, r_object;

    Vector3 prevLHandPos = Vector3.zero, prevRHandPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (leftHand == null) leftHand = GameObject.Find("LeftHand Controller");
        if (rightHand == null) rightHand = GameObject.Find("RightHand Controller");
    }

    // Update is called once per frame
    void Update()
    {
        //if (active) {
            // Get hand velocitys
            Vector3 l_handVelocity = (leftHand.transform.position - prevLHandPos) / Time.deltaTime;
            Vector3 r_handVelocity = (rightHand.transform.localPosition - prevRHandPos) / Time.deltaTime;
            prevLHandPos = leftHand.transform.position;
            prevRHandPos = rightHand.transform.localPosition;

            //print(l_handVelocity);
            l_object.AddForce(l_handVelocity);
            r_object.AddForce(r_handVelocity);
        //}
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            active = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            active = false;
        }
    }
}
