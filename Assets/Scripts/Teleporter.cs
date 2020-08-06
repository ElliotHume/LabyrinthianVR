using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject triggerObject;
    public GameObject teleportAnchor;

    public List<GameObject> toggleObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if ( other.gameObject == triggerObject ) {
            Teleport();
        }
    }

    void Teleport() {
        triggerObject.transform.position = teleportAnchor.transform.position;
        triggerObject.transform.rotation = teleportAnchor.transform.rotation;

        if (toggleObjects.Count > 0 ) {
            foreach( GameObject go in toggleObjects ) {
                go.SetActive(!go.activeInHierarchy);
            }
        }
    }
}
