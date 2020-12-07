using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpellComponent : MonoBehaviour
{
    public string effectName;
    public float lifetime = 0f;

    public GameObject breakFX;
    XRGrabInteractable grabInteractable;

    void Awake() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (lifetime > 0f) Invoke(nameof(Break), lifetime);
    }

    public void Break() {
        if (breakFX != null) Instantiate(breakFX, transform.position, transform.rotation);
        if (grabInteractable != null) grabInteractable.colliders.Clear();
        Destroy(gameObject);
    }

    public void Create(string newEffectName) {
        effectName = newEffectName;
    }

}
