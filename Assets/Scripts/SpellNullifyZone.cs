using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellNullifyZone : MonoBehaviour
{
    public AudioSource destroySound;
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Spell") {
            Destroy(other.gameObject);
            if (destroySound != null) destroySound.Play();
        }
    }
}
