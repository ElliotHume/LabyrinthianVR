using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBooster : MonoBehaviour
{
    public bool active = true;
    public float forceFactor = 1f;
    public ParticleSystem particles;
    public AudioSource sound;

    void OnTriggerStay(Collider other) {
        if (active) {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddForce(-Physics.gravity * forceFactor);
            }

            CharacterController cc = other.gameObject.GetComponent<CharacterController>();
            if (cc != null) {
                cc.Move(new Vector3(0, -Physics.gravity.y * forceFactor  * Time.deltaTime, 0));
            }
        }
    }

    public void Activate() {
        active = true;
        particles.Play();
        sound.Play();
    }

    public void Deactivate() {
        active = false;
        particles.Stop();
        sound.Stop();
    }
}
