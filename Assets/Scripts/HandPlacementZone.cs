using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HandPlacementZone : MonoBehaviour
{
    public Color baseColour, activeColour;
    public Image image;
    public float timedDeactivate = 0f;
    public ParticleSystem particles;
    public UnityEvent onActivate, onDeactivate;
    public AudioSource activateSound, deactivateSound;
    
    bool active = false, interrupt = false;

    public void Activate() {
        if (!active) {
            onActivate.Invoke();
            active = true;
            if (particles != null && !particles.isPlaying) particles.Play();
            if (image != null) image.color = activeColour;
            if (activateSound != null) activateSound.Play();
            if (timedDeactivate > 0f) {
                StartCoroutine(TimedDeactivate());
            }
        }
    }

    public void Deactivate() {
        onDeactivate.Invoke();
        if (particles != null) particles.Stop();
        if (image != null) image.color = baseColour;
        if (deactivateSound != null) deactivateSound.Play();
        active = false;
    }

    public void ForceVisualActivate() {
        active = true;
        if (image != null) image.color = activeColour;
        if (particles != null && !particles.isPlaying) particles.Play();
        if (activateSound != null) activateSound.Play();
        interrupt = true;
    }


    IEnumerator TimedDeactivate() {
        yield return new WaitForSeconds(timedDeactivate);
        if (!interrupt) Deactivate();
    }
}
