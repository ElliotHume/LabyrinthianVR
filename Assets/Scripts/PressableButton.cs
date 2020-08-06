﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class PressableButton : XRBaseInteractable
{
    public UnityEvent OnPress;
    private bool prevPress = false;
    private XRBaseInteractor hoverInteractor;
    private float prevHandHeight = 0f;
    private float yMin, yMax = 0f;


    // Start is called before the first frame update
    void Start()
    {
        SetMinMax();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    

    protected override void Awake() {
        base.Awake();
        onHoverEnter.AddListener(StartPress);
        onHoverExit.AddListener(EndPress);
    }

    void OnDestroy() {
        onHoverEnter.RemoveListener(StartPress);
        onHoverExit.RemoveListener(EndPress);    
    }

    void StartPress(XRBaseInteractor interactor) {
        hoverInteractor = interactor;
        prevHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
    }

    void EndPress(XRBaseInteractor interactor) {
        hoverInteractor = null;
        prevHandHeight = 0f;
        prevPress = false;
        SetYPosition(yMax);
    }

    void SetMinMax() {
        Collider collider = GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
        yMax = transform.localPosition.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
        if (hoverInteractor) {
            float newHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
            float handDiff = prevHandHeight - newHandHeight;
            prevHandHeight = newHandHeight;
            float newPosition = transform.localPosition.y - handDiff;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    float GetLocalYPosition(Vector3 position) {
        Vector3 localPos = transform.parent.InverseTransformPoint(position);
        return localPos.y;
    }

    void SetYPosition(float position) {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPosition;
    }

    void CheckPress() {
        bool inPosition = InPosition();
        if (inPosition && inPosition != prevPress)
            OnPress.Invoke();
        prevPress = inPosition;
    }

    bool InPosition() {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin+0.01f);
        return transform.localPosition.y == inRange;
    }
}