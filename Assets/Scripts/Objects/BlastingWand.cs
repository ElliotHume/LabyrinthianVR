using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlastingWand : XRGrabInteractable
{
    public GameObject prefab;
    public Transform spawningAnchor;
    XRController handController;
    float timeout = 0f;

    void Start() {
        StartCoroutine(TimeoutTimer());    
    }

    protected override void OnSelectEnter(XRBaseInteractor interactor) {
        base.OnSelectEnter(interactor);

        handController = gameObject.GetComponent<XRController>();
    }

    protected override void OnActivate(XRBaseInteractor interactor) {
        base.OnActivate(interactor);
        if (timeout == 0f) {
            Instantiate(prefab, spawningAnchor.position, spawningAnchor.rotation);
            if (handController != null) handController.SendHapticImpulse(1f, 0.2f);
            timeout = 0.5f;
        }
    }

    IEnumerator TimeoutTimer() {
        while (true) {
            if (timeout > 0f) {
                yield return new WaitForSeconds(timeout);
                timeout = 0f;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
