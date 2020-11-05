using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HandLinkPuzzle : MonoBehaviour
{
    public GameObject leftHand, rightHand;

    public GameObject l_object, r_object, goalZone, secondGoalZone;

    public UnityEvent onComplete;

    Rigidbody l_object_rb, r_object_rb;
    float error_tolerance = 1f;
    bool goalZoneIn = false, secondGoalZoneIn = false;
    Vector3 prevLHandPos = Vector3.zero, prevRHandPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (leftHand == null) leftHand = GameObject.Find("LeftHand Controller");
        if (rightHand == null) rightHand = GameObject.Find("RightHand Controller");

        l_object_rb = l_object.GetComponent<Rigidbody>();
        r_object_rb = r_object.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get hand velocitys
        Vector3 l_handVelocity = (leftHand.transform.position - prevLHandPos) / Time.deltaTime;
        Vector3 r_handVelocity = (rightHand.transform.localPosition - prevRHandPos) / Time.deltaTime;
        prevLHandPos = leftHand.transform.position;
        prevRHandPos = rightHand.transform.localPosition;

        //print(l_handVelocity);
        l_object_rb.AddForce(l_handVelocity * 2f);
        r_object_rb.AddForce(r_handVelocity * 2f);

        // Check if something is in the first goal zone
        if ((l_object.transform.position - goalZone.transform.position).magnitude <= error_tolerance || (r_object.transform.position - goalZone.transform.position).magnitude <= error_tolerance) {
            goalZoneIn = true;
        } else {
            goalZoneIn = false;
        }

        // Check if something is in the second goal zone
        if ((l_object.transform.position - secondGoalZone.transform.position).magnitude <= error_tolerance || (r_object.transform.position - secondGoalZone.transform.position).magnitude <= error_tolerance) {
            secondGoalZoneIn = true;
        } else {
            secondGoalZoneIn = false;
        }

        if (goalZoneIn && secondGoalZoneIn) {
            onComplete.Invoke();
            l_object_rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            r_object_rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
    }

}
