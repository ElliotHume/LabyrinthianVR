using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellNullifyZone : MonoBehaviour
{
    public float timer;
    public AudioSource destroySound;

    void Start() {
        if ( timer!= null || timer > 0f) Destroy(gameObject, timer);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spell" || other.tag == "Missile" || other.tag == "SpellPassthrough") {
            Destroy(other.gameObject);
            if (destroySound != null) destroySound.Play();
        }
    }
}
