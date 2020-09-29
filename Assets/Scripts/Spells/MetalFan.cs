using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MetalFan : MonoBehaviour
{
    public Missile[] spikes;
    public AudioSource castSound;
    
    bool released = false;

    public void Release() {
        if(!released) {
            if (castSound != null) castSound.Play();

            foreach (Missile spike in spikes){
                spike.Shoot();
            }
            released = true;
            
            Destroy(gameObject, 15f);
        }
    }
}
