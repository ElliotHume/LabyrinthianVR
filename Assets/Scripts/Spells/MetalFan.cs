using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MetalFan : MonoBehaviour
{
    public Missile[] spikes;
    public AudioSource castSound;
    bool released = false;
    XRGrabInteractable grabInteractable;

    void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void Release() {
        if(!released) {
            if (castSound != null) castSound.Play();

            foreach (Missile spike in spikes){
                spike.Shoot();
            }
            released = true;
            
            grabInteractable.colliders.Clear();
            Destroy(gameObject, 15f);
        }
    }
}
