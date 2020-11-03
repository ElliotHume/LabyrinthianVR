using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlastingWand : XRGrabInteractable
{
    public GameObject prefab;
    public Transform spawningAnchor;
    XRController handController;

    void FixedUpdate()
    {
        
    }

    protected override void OnSelectEnter(XRBaseInteractor interactor) {
        base.OnSelectEnter(interactor);

        handController = gameObject.GetComponent<XRController>();
    }

    protected override void OnActivate(XRBaseInteractor interactor) {
        base.OnActivate(interactor);
        Instantiate(prefab, spawningAnchor.position, spawningAnchor.rotation);
        if (handController != null) handController.SendHapticImpulse(1f, 0.2f);
    }

}
