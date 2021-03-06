﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    Vector3 resetPosition, rotationAngles;

    public bool bobs = true, rotates = true;
    public float bobbingSpeed, bobbingHeight;
    // Start is called before the first frame update
    void Start()
    {
        resetPosition = transform.position;
        rotationAngles = new Vector3(Random.Range(0f, 0.1f), Random.Range(0f, 0.1f), Random.Range(0f, 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (rotates) transform.Rotate(rotationAngles);
        if (bobs){
            //calculate what the new Y position will be
            float newY = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight + resetPosition.y;
            //set the object's Y to the new calculated Y
            transform.position = new Vector3(transform.position.x, newY, transform.position.z) ;
        }
    }

    public void StartMoving() {
        bobs = true;
        rotates = true;
    }

    public void StopMoving() {
        bobs = false;
        rotates = false;
    }
}
