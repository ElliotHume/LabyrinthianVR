using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetPlane : MonoBehaviour
{
    public string targetTag;
    public string targetName;
    public UnityEvent OnEnter, OnExit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == targetTag || other.gameObject.name == targetName) {
            OnEnter.Invoke();

            AudioSource asource = GetComponent<AudioSource>();
            if (asource != null) asource.Play();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == targetTag || other.gameObject.name == targetName) {
            OnExit.Invoke();
            // AudioSource asource = GetComponent<AudioSource>();
            // if (asource != null) asource.Play();
        }
    }

}
