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
            other.gameObject.transform.position = transform.position + new Vector3(-0.036f, 0.12f, -0.034f); 

            GetComponent<AudioSource>().Play();

            toggleObject.SetActive(true);
            print(toggleObject+" set to active "+toggleObject.activeInHierarchy+ " pos: "+toggleObject.transform.position);
        }
    }
}
