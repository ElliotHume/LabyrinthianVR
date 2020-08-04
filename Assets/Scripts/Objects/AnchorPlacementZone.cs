using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPlacementZone : MonoBehaviour
{
    public GameObject toggleObject;
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
        if (other.name == "DrawingAnchor") {
            other.gameObject.transform.position = transform.position + new Vector3(0f, 0.45f, 0f); 

            GetComponent<AudioSource>().Play();

            toggleObject.SetActive(!toggleObject.activeInHierarchy);
        }
    }
}
