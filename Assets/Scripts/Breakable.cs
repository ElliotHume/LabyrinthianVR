using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public float duration;
    public UnityEvent onBreak;
    Rigidbody rigidBody;
    bool broken = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break() {
        if (!broken) {
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
            ParticleSystem ps = GetComponent<ParticleSystem>();
            if (ps != null ) ps.Play();
            if (duration > 0) Destroy(gameObject, duration);
            onBreak.Invoke();
        } else {
            Destroy(gameObject, 0f);
        }
    }
}
