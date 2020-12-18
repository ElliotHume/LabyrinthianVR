using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpellComponent : MonoBehaviour
{
    public AuricaSpellComponent component;
    public float lifetime = 0f;

    public GameObject breakFX;
    public List<AuricaSpellComponent> allComponents = new List<AuricaSpellComponent>();
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
        foreach (AuricaSpellComponent item in allComponents){
            if (item.name == newEffectName) {
                component = item;
                break;
            }
        }
    }

}
